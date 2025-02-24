using CurrieTechnologies.Razor.SweetAlert2;
using FaxMailFrontend.Data;
using FaxMailFrontend.ViewModel;
using MailDLL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

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

		protected override async Task OnInitializedAsync()
		{
			basePath = env.WebRootPath + "\\Files";
			MaxFileSize = GetMaxMB() * 1024 * 1024;
			MaxFilesPerStack = GetMaxFilesPerStack();
		}

		private void Logout()
		{
			// Implementiere die Logout-Logik hier
			navigationManager.NavigateTo("/LogOutPage");
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
					//logger.LogError("Dropzone initialized");
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
					using var stream = file.OpenReadStream(MaxFileSize);
					var path = Path.Combine(env.WebRootPath, "Files", usedPathname, file.Name);
					if (File.Exists(path))
					{
						path = Path.Combine(env.WebRootPath, "Files", usedPathname, Path.GetFileNameWithoutExtension(file.Name)+ $"_re{uploadcounter}" + Path.GetExtension(file.Name) );
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
						fileHandler.AddFile(Path.GetFileName(path), File.ReadAllBytes(path));
					}
				}
				//fileHandler = new FileHandler($"{env.WebRootPath}\\Files\\{usedPathname}\\", usedPathname);

				if (fileHandler.Files.Count > MaxFilesPerStack)
				{
					foreach (var file in files)
					{
						fileHandler.DeleteFile(file.Name);
					}

					messageText = $"Es d�rfen maximal {MaxFilesPerStack} Dateien hochgeladen werden." + Environment.NewLine + "Die zuletzt zugef�gten Deteien wurden gel�scht";
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
						Text = $"Die Anzahl der �bertragenen Daten ist gr��er als das Maximum von {MaxFilesPerStack}",
						Icon = SweetAlertIcon.Warning,
						ConfirmButtonText = "OK"
					});
				}
				else if (ex.Message.Contains("exceeds the maximum of"))
				{
					var result = await Swal.FireAsync(new SweetAlertOptions
					{
						Title = "Achtung!",
						Text = $"Die bertragene Datei ist gr��er als das Maximum von {MaxFileSize} Bytes",
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

		private async Task EmailSplitter(string filename, string path)
		{
			try
			{
				Mail myMail = new Mail(Path.Combine(path, filename));
				myMail.CollectAttachments();
				List<string> filelistFromMail = myMail.WriteAttachments();
				foreach (var file in filelistFromMail)
				{
					fileHandler.AddFile(Path.GetFileName(file), File.ReadAllBytes(file));
				}
			}
			catch (Exception ex)
			{
				eh.Systemmessage = ex.Message;
				eh.EC = ErrorCode.DatenKonntenNichtKopiertWerden;
				navigationManager.NavigateTo($"/ErrorPage");
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
					string? message = FileHandler.CheckFile(path);
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
							Text = file.Message + Environment.NewLine + "Das File wurde gel�scht.",
							Icon = SweetAlertIcon.Info,
							ConfirmButtonText = "OK"
						});
						//await JSRuntime.InvokeVoidAsync("alert", file.Message + Environment.NewLine + "Das File wurde gel�scht.");
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
		private void HandleFileHandlerChanged(FileHandler updatedFileHandler)
		{
			fileHandler = updatedFileHandler;
			//if (fileHandler.Files.Count == 0)
			selectedFile = null;
			CheckIfAbsendenErlaubt();
			StateHasChanged();
		}
		private void HandlePathNameChanged(FileInformation file)
		{
			selectedFile = file;

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
			if (fileHandler != null && fileHandler.Files.All(file => file.isSaved))
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

			//XML Datei pro File erstelle

			//Files kopieren

			//Folder L�schen
		}


		//private void DeleteFile(string filename)
		//{
		//	try
		//	{
		//		if (fileHandler is not null)
		//			fileHandler.DeleteFile(filename);
		//		File.Delete($"{env.WebRootPath}\\Files\\{usedPathname}\\{filename}");
		//	}
		//	catch (Exception ex)
		//	{
		//		eh.Systemmessage = ex.Message;
		//		eh.EC = ErrorCode.KeineDateiGeloescht;
		//		navigationManager.NavigateTo($"/ErrorPage");
		//	}
		//}

		private void Refresh()
		{
			navigationManager.NavigateTo("/EmailSinglePage", true);
		}

		private void ResetPage()
		{
			
			_module = null;
			_dropzoneInstance = null;
			messageText = "Keine Fehler";
			Refresh();
		}
	}
}