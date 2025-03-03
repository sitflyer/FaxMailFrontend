using CurrieTechnologies.Razor.SweetAlert2;
using FaxMailFrontend.Data;
using FaxMailFrontend.ViewModel;
using MailDLL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;

using XMLBox;

namespace FaxMailFrontend.Pages
{
	public partial class EmailSinglePage
	{
		int MaxFileSize = 1;
		int MaxFilesPerStack = 1;
		IJSObjectReference? _module;
		IJSObjectReference? _dropzoneInstance;
		ElementReference dropZoneElement;
		ElementReference inputFileContainer;

		private string messageText = "";
		private bool success = false;
		private string username = "Cindy Kassab";

		private string telefonnummer = "0171-1272712";
		private string mail = "cindy.kassab@aok.de";
		private bool absendenVerboten = true;
		private string FilePath { get; set; } = "";
		private string usedPathname = "";
		private FileInformation? selectedFile;
		private string basePath = "";
		private int uploadcounter = 0;
		private IBrowserFile filemerker;
		private bool protector = false;
		private bool loggedOut = false;
		protected override async Task OnInitializedAsync()
		{

			basePath = env.WebRootPath + "\\Files";
			MaxFileSize = GetMaxMB() * 1024 * 1024;
			MaxFilesPerStack = GetMaxFilesPerStack();
			if (Configuration.GetValue<string>("FileSettings:Targetfolder") != null)
			{
				fileHandler.Targetfolder = Configuration.GetValue<string>("FileSettings:Targetfolder")!;
			}
			else
			{
				logger.LogError("Targetfolder not found in appsettings.json");
				eh.Systemmessage = "Targetfolder not found in appsettings.json";
				eh.EC = ErrorCode.VerzeichnisKonnteNichtAngelegtWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
			if (Configuration.GetValue<string>("FileSettings:Protokollfolder") != null)
			{
				fileHandler.Protokollfolder = Configuration.GetValue<string>("FileSettings:Protokollfolder")!;
			}
			else
			{
				logger.LogError("Protokollfolder not found in appsettings.json");
				eh.Systemmessage = "Protokollfolder not found in appsettings.json";
				eh.EC = ErrorCode.VerzeichnisKonnteNichtAngelegtWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
			if (!Directory.Exists(fileHandler.Protokollfolder))
			{
				logger.LogError("Protokollfolder existiert nicht.");
				eh.Systemmessage = "Protokollfolder existiert nicht.";
				eh.EC = ErrorCode.VerzeichnisKonnteNichtAngelegtWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
			if (!Directory.Exists(fileHandler.Targetfolder))
			{
				logger.LogError("Targetfolder existiert nicht.");
				eh.Systemmessage = "Targetfolder existiert nicht.";
				eh.EC = ErrorCode.VerzeichnisKonnteNichtAngelegtWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
		}

		private async Task Logout()
		{
			await JSRuntime.InvokeVoidAsync("navigateToExternalUrl", "MicrosoftIdentity/Account/SignOut");
		}
		public int GetMaxMB()
		{
			return Configuration.GetValue<int>("FileSettings:MaxMB");
		}
		public int GetMaxFilesPerStack()
		{
			return Configuration.GetValue<int>("FileSettings:MaxFilesPerStack");
		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				try
				{
					_module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/dropZone.js");
					_dropzoneInstance = await _module.InvokeAsync<IJSObjectReference>("initializeFileDropZone", dropZoneElement, inputFileContainer);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

		async Task OnChange(InputFileChangeEventArgs e)
		{
			if (usedPathname == "")
			{
				usedPathname = fileHandler.UUID;

				basePath = Path.Combine(env.WebRootPath, "Files", usedPathname);
				try
				{
					var newDirectoryPath = Path.Combine(env.WebRootPath, "Files", usedPathname);
					Directory.CreateDirectory(newDirectoryPath);
				}
				catch (Exception ex)
				{
					eh.Systemmessage = ex.Message;
					eh.EC = ErrorCode.VerzeichnisKonnteNichtAngelegtWerden;
					navigationManager.NavigateTo($"/ErrorPage");
				}
			}
			try
			{
				var files = e.GetMultipleFiles(MaxFilesPerStack);
				foreach (var file in files)
				{
					filemerker = file;
					using var stream = file.OpenReadStream(MaxFileSize);
					var path = Path.Combine(env.WebRootPath, "Files", usedPathname, file.Name);
					if (File.Exists(path))
					{
						path = Path.Combine(env.WebRootPath, "Files", usedPathname, Path.GetFileNameWithoutExtension(file.Name) + $"_re{uploadcounter}" + Path.GetExtension(file.Name));
						uploadcounter++;
					}
					FileStream fileStream = File.Create(path);
					await stream.CopyToAsync(fileStream);
					stream.Close();
					fileStream.Close();

					if (Path.GetExtension(file.Name).Equals(".eml", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(file.Name).Equals(".msg", StringComparison.OrdinalIgnoreCase))
					{
						await EmailSplitter(file.Name, Path.Combine(env.WebRootPath, "Files", usedPathname));
						fileHandler.DeleteFile(file.Name);
					}
					else
					{
						fileHandler.AddFile(Path.GetFileName(path), File.ReadAllBytes(path), fileHandler.Files.Count);
						CheckIfAbsendenErlaubt();
					}
				}

				if (fileHandler.Files.Count > MaxFilesPerStack)
				{
					foreach (var file in files)
					{
						fileHandler.DeleteFile(file.Name);
					}

					messageText = $"Es dürfen maximal {MaxFilesPerStack} Dateien hochgeladen werden." + Environment.NewLine + "Die zuletzt zugefügten Deteien wurden gelöscht";
					var result = await Swal.FireAsync(new SweetAlertOptions
					{
						Title = "Achtung!",
						Text = messageText,
						Icon = SweetAlertIcon.Warning,
						ConfirmButtonText = "OK"
					});
				}
				await RunChecks();
				StateHasChanged();
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("The maximum number of files accepted"))
				{
					var result = await Swal.FireAsync(new SweetAlertOptions
					{
						Title = "Achtung!",
						Text = $"Die Anzahl der übertragenen Daten ist größer als das Maximum von {MaxFilesPerStack}",
						Icon = SweetAlertIcon.Warning,
						ConfirmButtonText = "OK"
					});
				}
				else if (ex.Message.Contains("exceeds the maximum of"))
				{
					var result = await Swal.FireAsync(new SweetAlertOptions
					{
						Title = "Achtung!",
						Text = $"Die übertragene Datei \"{Path.GetFileName(filemerker.Name)}\" ist größer als das Maximum von {MaxFileSize} Bytes",
						Icon = SweetAlertIcon.Warning,
						ConfirmButtonText = "OK"
					});
				}
				else
				{
					eh.Systemmessage = ex.Message;
					eh.EC = ErrorCode.DatenKonntenNichtKopiertWerden;
					navigationManager.NavigateTo($"/ErrorPage");
				}
			}
		}

		private async Task OnBeforeNavigation(LocationChangingContext context)
		{
			if (!protector)
			{
				if (fileHandler.Files.Count > 0)
				{
					var result = Swal.FireAsync(new SweetAlertOptions
					{
						Title = "Achtung!",
						Text = "Interne Navigation ist nicht erlaubt!",
						Icon = SweetAlertIcon.Info,
						ConfirmButtonText = "OK"
					});
					context.PreventNavigation();
				}
			}
			protector = false;
		}
		private async Task EmailSplitter(string filename, string path)
		{
			try
			{
				Mail myMail = new Mail(Path.Combine(path, filename));
				myMail.CollectAttachments();
				List<string> filelistFromMail = myMail.WriteAttachments();
				int count = 0;
				foreach (var file in filelistFromMail)
				{
					fileHandler.AddFile(Path.GetFileName(file), File.ReadAllBytes(file), count);
					count++;
				}
			}
			catch
			{
				var result = Swal.FireAsync(new SweetAlertOptions
				{
					Title = "Achtung!",
					Text = "Die eingereichte E-Mail enthält nicht auslesbaren Inhalt.Bitte die Dokumente einzeln einreichen.",
					Icon = SweetAlertIcon.Info,
					ConfirmButtonText = "OK"
				});
			}
		}
		private struct FileCheckResult
		{
			public string? Message { get; set; }
			public string File { get; set; }
		}
		private async Task RunChecks()
		{
			List<FileCheckResult> filelistToDelete = new List<FileCheckResult>();
			try
			{
				foreach (var file in fileHandler.Files)
				{
					string path = Path.Combine(env.WebRootPath, "Files", usedPathname, file.FileName);
					string? message = FileHandler.CheckFile(path, MaxFileSize);
					if (message != null)
					{
						filelistToDelete.Add(new FileCheckResult { Message = message, File = file.FileName });

					}
				}
				if (filelistToDelete.Count > 0)
				{
					foreach (var file in filelistToDelete)
					{
						fileHandler.DeleteFile(file.File);
						var result = await Swal.FireAsync(new SweetAlertOptions
						{
							Title = "Achtung!",
							Text = file.Message + Environment.NewLine + "Das File wurde gelöscht.",
							Icon = SweetAlertIcon.Info,
							ConfirmButtonText = "OK"
						});
						//await JSRuntime.InvokeVoidAsync("alert", file.Message + Environment.NewLine + "Das File wurde gelöscht.");
					}
				}
			}
			catch (Exception ex)
			{
				eh.Systemmessage = ex.Message;
				eh.EC = ErrorCode.DatenKonntenNichtKopiertWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
		}
		//private void HandleFileHandlerChanged(FileHandler updatedFileHandler)
		//{
		//	fileHandler = updatedFileHandler;
		//	//if (fileHandler.Files.Count == 0)
		//	selectedFile = null;
		//	CheckIfAbsendenErlaubt();
		//	StateHasChanged();
		//}
		private void HandleTestFilehandler(FileHandler updatedFileHandler)
		{
			fileHandler = updatedFileHandler;
			CheckIfAbsendenErlaubt();
			StateHasChanged();
		}
		//private void HandleFileUpdate(FileInformation file)
		//{
		//	selectedFile = file;
		//	StateHasChanged();
		//}

		private void HandlePathNameChanged(FileInformation file)
		{
			selectedFile = file;
			CheckIfAbsendenErlaubt();
			StateHasChanged();
		}
		protected override void OnInitialized()
		{
			base.OnInitialized();
			InitializeFileHandler();
		}

		private void InitializeFileHandler()
		{
			fileHandler.UUID = Guid.NewGuid().ToString();
			fileHandler.ErrorMessage = "";
			fileHandler.Path = $"{env.WebRootPath}\\Files";
		}

		private void CheckIfAbsendenErlaubt()
		{
			if (fileHandler != null && fileHandler.Files.All(file => file.isSaved) && fileHandler.Files.Count != 0)
			{
				absendenVerboten = false;
			}
			else
			{
				absendenVerboten = true;
			}
		}

		private async Task Absenden()
		{
			protector = true;
			
			try
			{
				await CopyAllFileToOuput();
			}
			catch (Exception ex)
			{
				eh.Systemmessage = ex.Message;
				eh.EC = ErrorCode.DatenKonntenNichtKopiertWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
			success = true;
			StateHasChanged();
		}

		private async Task CopyAllFileToOuput()
		{
			//Files benennen
			try
			{
				int counter = 0;
				foreach (var file in fileHandler.Files)
				{
					string orgfile = Path.Combine(env.WebRootPath, "Files", fileHandler.UUID, file.FileName);
					string extension = Path.GetExtension(orgfile);
					string kennung = counter.ToString("D3");
					string zielfilename = fileHandler.UUID + "_" + kennung + extension;
					string ziel = Path.Combine(fileHandler.Targetfolder, fileHandler.Targetfolder, zielfilename);
					string sicherheit = Path.Combine(fileHandler.Targetfolder, fileHandler.Protokollfolder, zielfilename);
					File.Copy(Path.Combine(env.WebRootPath, "Files", fileHandler.UUID, fileHandler.Files[0].FileName), ziel);
					File.Copy(Path.Combine(env.WebRootPath, "Files", fileHandler.UUID, fileHandler.Files[0].FileName), sicherheit);
					await CreateXMLFile(ziel, file);

					counter++;
				}
				await DeleteWorkFolder();
			}
			catch (Exception ex)
			{
				eh.Systemmessage = ex.Message;
				eh.EC = ErrorCode.DatenKonntenNichtKopiertWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
		}

		private async Task CreateXMLFile(string ziel, FileInformation file)
		{
			string output = Path.GetFileNameWithoutExtension(ziel) + ".xml";
			string outputpath = Path.Combine(fileHandler.Targetfolder, output);
			string protokollpath = Path.Combine(fileHandler.Protokollfolder, output);
			List<string> DokumentZeilen = new() { };
			DokumentZeilen.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			DokumentZeilen.Add("<email>");
			DokumentZeilen.Add("<inboundchannel>EM</inboundchannel>");
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "receivedDate", DateTime.Now.ToString("dd-MM-yyyy")));
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "processtimestamp", DateTime.Now.ToString("dd-MM-yyyy")));
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_SentFrom", fileHandler.Vorname + " " + fileHandler.Name));
			if (file.KanalArt == KanalArt.EPost)
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_SentTo", "Pfad_EPost"));
			}
			else if (file.KanalArt == KanalArt.lateScan)
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_SentTo", "Pfad_Latescan"));
			}
			else
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_SentTo", "Pfad_MedicalDocument"));
			}
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_Dokumentart", file.DokumentenKlasse));
			if (string.IsNullOrEmpty(file.KVNR))
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_KVNummer", file.KVNR));
			}
			if (string.IsNullOrEmpty(file.BTNR))
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_BTNR", file.BTNR));
			}
			if (string.IsNullOrEmpty(file.GPNR))
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_BPNR", file.GPNR));
			}
			if (string.IsNullOrEmpty(file.LEIK))
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_Leik", file.LEIK));
			}
			if (string.IsNullOrEmpty(file.Fallbuendelnummer))
			{
				DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_BOID", file.Fallbuendelnummer));
			}
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_Filename", file.FileName));
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_Fsname", Path.GetFileName(ziel)));
			FileInfo fileInfo = new FileInfo(ziel);
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_Filesize", fileInfo.Length));
			DokumentZeilen.Add("</email>");
			TextDokument textDokument = new TextDokument(outputpath, DokumentZeilen);

			try
			{
				textDokument.DokumentSchreiben(true);
				File.Copy(outputpath, protokollpath);
			}
			catch (Exception ex)
			{
				eh.Systemmessage = ex.Message;
				eh.EC = ErrorCode.DatenKonntenNichtKopiertWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
		}

		private void Refresh()
		{
			protector = true;
			navigationManager.NavigateTo("/EmailSinglePage", true);
		}

		private async Task ResetPage()
		{
			protector = true;
			_module = null;
			_dropzoneInstance = null;
			messageText = "Keine Fehler";
			await DeleteWorkFolder();
			Refresh();
		}
		private async Task DeleteWorkFolder()
		{
			try
			{
				string path = Path.Combine(env.WebRootPath, "Files", fileHandler.UUID);
				//var result = await Swal.FireAsync(new SweetAlertOptions
				//{
				//	Title = "Achtung!",
				//	Text = $"{path} wurde gelöscht.",
				//	Icon = SweetAlertIcon.Info,
				//	ConfirmButtonText = "OK"
				//});
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
				//Directory.Delete(Path.Combine($env.WebRootPath, "Files", fileHandler.UUID), true);
			}
			catch (Exception ex)
			{

			}


		}
	}
}