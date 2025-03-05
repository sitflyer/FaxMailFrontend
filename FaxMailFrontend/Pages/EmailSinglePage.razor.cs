using CurrieTechnologies.Razor.SweetAlert2;
using DataAccessDLL.Modell;
using DataAccessDLL.Services;
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
using System.Security.Claims;
using XMLBox;

namespace FaxMailFrontend.Pages
{
	public partial class EmailSinglePage
	{
		int MaxFileSize = 1;
		int MaxFilesPerStack = 1;
		int MaxPagesPerPDF = 1;
		IJSObjectReference? _module;
		IJSObjectReference? _dropzoneInstance;
		ElementReference dropZoneElement;
		ElementReference inputFileContainer;

		private string messageText = "";
		private bool success = false;
		private const string username = "Cindy Kassab";

		private const string telefonnummer = "0171-1272712";
		private const string mail = "cindy.kassab@aok.de";
		private bool absendenVerboten = true;
		private string FilePath { get; set; } = "";
		private string usedPathname = "";
		private FileInformation? selectedFile;
		private string basePath = "";
		private int uploadcounter = 0;
		private IBrowserFile? filemerker;
		private bool protector = false;
		private Nutzer? nutzer = null;
		protected override async Task OnInitializedAsync()
		{
			base.OnInitialized();
			await InitializeFileHandler();
			try
			{
				basePath = env.WebRootPath + "\\Files";
				MaxFileSize = GetMaxMB() * 1024 * 1024;
				MaxFilesPerStack = GetMaxFilesPerStack();
				MaxPagesPerPDF = Configuration.GetValue<int>("FileSettings:MaxPagesPerPDF");

				if (Configuration.GetValue<string>("FileSettings:EPostFolder") != null)
				{
					fileHandler.EPostFolder = Configuration.GetValue<string>("FileSettings:EPostFolder")!;
				}
				else
				{
					ErrorHandle("EPostFolder existiert nicht.", ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
				}
				if (Configuration.GetValue<string>("FileSettings:LateScanFolder") != null)
				{
					fileHandler.LateScanFolder = Configuration.GetValue<string>("FileSettings:LateScanFolder")!;
				}
				else
				{
					ErrorHandle("LateScanFolder existiert nicht.", ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
				}
				if (Configuration.GetValue<string>("FileSettings:Protokollfolder") != null)
				{
					fileHandler.Protokollfolder = Configuration.GetValue<string>("FileSettings:Protokollfolder")!;
				}
				else
				{
					ErrorHandle("Protokollfolder not found in appsettings.json", ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
				}
				if (!Directory.Exists(fileHandler.Protokollfolder))
				{
					ErrorHandle("Protokollfolder existiert nicht.", ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
				}
				if (!Directory.Exists(fileHandler.EPostFolder))
				{
					ErrorHandle("EPostFolder existiert nicht.", ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
				}
				if (!Directory.Exists(fileHandler.LateScanFolder))
				{
					ErrorHandle("LateScanFolder existiert nicht.", ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
				}
			}
			catch (Exception ex)
			{
				ErrorHandle(ex.Message, ErrorCode.AllgemeinerFehler);
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
					ErrorHandle(ex.Message, ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
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
					ErrorHandle(ex.Message, ErrorCode.VerzeichnisKonnteNichtAngelegtWerden);
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
					await Swal.FireAsync(new SweetAlertOptions
					{
						Title = "Achtung!",
						Text = $"Die Anzahl der übertragenen Daten ist größer als das Maximum von {MaxFilesPerStack}",
						Icon = SweetAlertIcon.Warning,
						ConfirmButtonText = "OK"
					});
				}
				else if (ex.Message.Contains("exceeds the maximum of"))
				{
					await Swal.FireAsync(new SweetAlertOptions
					{
						Title = "Achtung!",
						Text = $"Die übertragene Datei \"{Path.GetFileName(filemerker!.Name)}\" ist größer als das Maximum von {MaxFileSize} Bytes",
						Icon = SweetAlertIcon.Warning,
						ConfirmButtonText = "OK"
					});
				}
				else
				{
					ErrorHandle(ex.Message, ErrorCode.DatenKonntenNichtKopiertWerden);
				}
			}
		}

		private async Task OnBeforeNavigation(LocationChangingContext context)
		{
			if (!protector)
			{
				if (fileHandler.Files.Count > 0)
				{
					await Swal.FireAsync(new SweetAlertOptions
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
				Mail myMail = new(Path.Combine(path, filename));
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
				await Swal.FireAsync(new SweetAlertOptions
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
			List<FileCheckResult> filelistToDelete = [];
			try
			{
				foreach (var file in fileHandler.Files)
				{
					string path = Path.Combine(env.WebRootPath, "Files", usedPathname, file.FileName);
					string? message = FileHandler.CheckFile(path, MaxFileSize, MaxPagesPerPDF);
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
				ErrorHandle(ex.Message, ErrorCode.DatenKonntenNichtKopiertWerden);
			}
		}

		private void HandleTestFilehandler(FileHandler updatedFileHandler)
		{
			fileHandler = updatedFileHandler;
			CheckIfAbsendenErlaubt();
			StateHasChanged();
		}
		private void HandlePathNameChanged(FileInformation file)
		{
			selectedFile = file;
			CheckIfAbsendenErlaubt();
			StateHasChanged();
		}

		private async Task InitializeFileHandler()
		{
			fileHandler.UUID = Guid.NewGuid().ToString();
			fileHandler.ErrorMessage = "";
			fileHandler.Path = $"{env.WebRootPath}\\Files";
			if (nutzer == null)
			{
				nutzer = new Nutzer();
				nutzer.Kennung = httpContextAccessor.HttpContext!.User.Identity!.Name;
				if (httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Email)?.Value is null)
				{
					nutzer.Email = "keine EMail zugewiesen!";
				}
				else
				{
					nutzer.Email = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Email)?.Value!;
				}
				if (httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.GivenName)?.Value is null)
				{
					nutzer.Vorname = "kein Vorname zugewiesen!";
				}
				else
				{
					nutzer.Vorname = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.GivenName)?.Value!;
				}
				if (httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Surname)?.Value is null)
				{
					nutzer.Nachname = "kein Nachname zugewiesen!";
				}
				else
				{
					nutzer.Nachname = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Surname)?.Value!;
				}

				nutzer.Telefonnummer = "keine Telefonnummer zugewiesen!";

				bool nutzerExists = await favoritenService.IsNutzerExisting(nutzer.Kennung!);
				if (nutzerExists)
				{
					nutzer.Id = await favoritenService.GetUserIDFromKennung(nutzer.Kennung!);
				}
				else
				{
					nutzer.Id = await favoritenService.CreateNutzerByKennungAndReturnNewID(nutzer);
				}
			}
			fileHandler.Nutzer = nutzer;
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
				ErrorHandle(ex.Message, ErrorCode.DatenKonntenNichtKopiertWerden);
			}
			success = true;
			StateHasChanged();
		}

		private void ErrorHandle(string ex, ErrorCode ec)
		{
			DeleteWorkFolder();
			logger.LogError(DateTime.Now.ToString() + " => " + ex);
			eh.Systemmessage = ex;
			eh.EC = ec;
			navigationManager.NavigateTo($"/ErrorPage");
		}
		private async Task CopyAllFileToOuput()
		{
			//Files benennen
			try
			{
				string ziel = "";
				int counter = 0;
				foreach (var file in fileHandler.Files)
				{
					string orgfile = Path.Combine(env.WebRootPath, "Files", fileHandler.UUID, file.FileName);
					string extension = Path.GetExtension(orgfile);
					string kennung = counter.ToString("D3");
					string zielfilename = fileHandler.UUID + "_" + kennung + extension;
					ziel = "";
					if (file.KanalArt == KanalArt.EPost)
					{
						ziel = Path.Combine(fileHandler.EPostFolder, zielfilename);
					}
					else if (file.KanalArt == KanalArt.lateScan)
					{
						ziel = Path.Combine(fileHandler.LateScanFolder, zielfilename);
					}
					else
					{
						ziel = Path.Combine(fileHandler.EPostFolder, zielfilename);
					}
					string sicherheit = Path.Combine(fileHandler.Protokollfolder, zielfilename);
					File.Copy(Path.Combine(env.WebRootPath, "Files", fileHandler.UUID, fileHandler.Files[0].FileName), ziel);
					File.Copy(Path.Combine(env.WebRootPath, "Files", fileHandler.UUID, fileHandler.Files[0].FileName), sicherheit);
					CreateXMLFile(ziel, file);
					await ReportToDB(file, ziel);
					ReportToLogger(file, ziel);
					counter++;
				}
				DeleteWorkFolder();
				string readypath = Path.GetDirectoryName(ziel)!;
				string rdyFile = Path.Combine(readypath, $"{fileHandler.UUID}.rdy");
				File.Create(rdyFile);
			}
			catch (Exception ex)
			{
				ErrorHandle(ex.Message, ErrorCode.DatenKonntenNichtKopiertWerden);
			}
		}

		private void ReportToLogger(FileInformation file, string zielfilename)
		{
			logger.LogInformation("Datum: {Date} Einreicher: {Einreicher} Filename: {Filename} Exportname: {Kennung}",
				DateTime.Now, fileHandler.Nutzer.Kennung, file.FileName, zielfilename);
		}

		private async Task ReportToDB(FileInformation file, string ziel)
		{
			try
			{
				await reportingService.AddReport(new Frontendreport
				{
					Nutzerkennung = fileHandler.Nutzer.Kennung!,
					EingereichtAm = DateTime.Now,
					Dokumenttyp = Path.GetExtension(file.FileName),
					Originalname = file.FileName,
					FinalerName = Path.GetFileName(ziel),
					Kanalart = file.KanalArt.ToString(),
					Dokumentenklasse = file.DokumentenKlasse,
					Kvnr = file.KVNR,
					Bpnr = file.GPNR,
					Btnr = file.BTNR,
					Boid = file.Fallbuendelnummer,
					Produktgruppe = file.Produktgruppe,
					Filesize = new FileInfo(ziel).Length.ToString()
				});
			}
			catch (Exception ex)
			{
				ErrorHandle(ex.Message, ErrorCode.DatenKonntenNichtKopiertWerden);
			}
			return;
		}

		private void CreateXMLFile(string ziel, FileInformation file)
		{
			string output = Path.GetFileNameWithoutExtension(ziel) + ".xml";
			string outputpath = Path.Combine(Path.GetDirectoryName(ziel)!, output);
			string protokollpath = Path.Combine(fileHandler.Protokollfolder, output);
			List<string> DokumentZeilen =
			[
				"<?xml version=\"1.0\" encoding=\"utf-8\"?>",
				"<email>",
				"<inboundchannel>EM</inboundchannel>",
				string.Format("<{0}>{1}</{0}>", "receivedDate", DateTime.Now.ToString("dd-MM-yyyy")),
				string.Format("<{0}>{1}</{0}>", "processtimestamp", DateTime.Now.ToString("dd-MM-yyyy")),
				string.Format("<{0}>{1}</{0}>", "CV_FaxMail_SentFrom", fileHandler.Nutzer.Vorname + " " + fileHandler.Nutzer.Nachname),
			];
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
			FileInfo fileInfo = new(ziel);
			DokumentZeilen.Add(string.Format("<{0}>{1}</{0}>", "CV_FaxMail_Filesize", fileInfo.Length));
			DokumentZeilen.Add("</email>");
			TextDokument textDokument = new(outputpath, DokumentZeilen);

			try
			{
				textDokument.DokumentSchreiben(true);
				File.Copy(outputpath, protokollpath);
			}
			catch (Exception ex)
			{
				ErrorHandle(ex.Message, ErrorCode.DatenKonntenNichtKopiertWerden);
			}
		}

		private async Task Refresh()
		{
			await InitializeFileHandler(); 
			protector = true;
			navigationManager.NavigateTo("/EmailSinglePage", true);
		}

		private async Task ResetPage()
		{
			protector = true;
			_module = null;
			_dropzoneInstance = null;
			messageText = "Keine Fehler";
			DeleteWorkFolder();
			await Refresh();
		}
		private void DeleteWorkFolder()
		{
			try
			{
				string path = Path.Combine(env.WebRootPath, "Files", fileHandler.UUID);
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
			}
			catch
			{
			}
		}
	}
}