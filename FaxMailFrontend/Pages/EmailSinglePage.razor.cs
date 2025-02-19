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
		private string? dokumentenPath;
		private string basePath = "";

		protected override async Task OnInitializedAsync()
		{
			basePath = env.WebRootPath + "\\Files";
			MaxFileSize = GetMaxMB() * 1024 * 1024;
			
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
				usedPathname = Guid.NewGuid().ToString();
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
				var files = e.GetMultipleFiles();
				foreach (var file in files)
				{
					using var stream = file.OpenReadStream(MaxFileSize);
					var path = Path.Combine(env.WebRootPath, "Files", usedPathname, file.Name);
					if (File.Exists(path))
					{
						File.Delete(path);
						messageText += $"Datei {Path.GetFileName(path)} wurde überschrieben";
					}
					FileStream fileStream = File.Create(path);
					await stream.CopyToAsync(fileStream);
					stream.Close();
					fileStream.Close();

					if (Path.GetExtension(file.Name).Equals(".eml", StringComparison.OrdinalIgnoreCase) || Path.GetExtension(file.Name).Equals(".msg", StringComparison.OrdinalIgnoreCase))
					{
						await EmailSplitter(file.Name, Path.Combine(env.WebRootPath, "Files", usedPathname));
					}
				}
				fileHandler = new FileHandler($"{env.WebRootPath}\\Files\\{usedPathname}\\", usedPathname);
				await RunChecks();
				StateHasChanged();
			}
			catch (Exception ex)
			{
				eh.Systemmessage = ex.Message;
				eh.EC = ErrorCode.DatenKonntenNichtKopiertWerden;
				navigationManager.NavigateTo($"/ErrorPage");
			}
		}

		private async Task EmailSplitter(string filename, string path)
		{
			try
			{
				Mail myMail = new Mail(Path.Combine(path, filename));
				myMail.CollectAttachments();
				myMail.WriteAttachments();
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
						DeleteFile(file.File);
						await JSRuntime.InvokeVoidAsync("alert", file.Message + Environment.NewLine + "Das File wurde gelöscht.");
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
			CheckIfAbsendenErlaubt();
			StateHasChanged();
		}
		private void HandlePathNameChanged(string path)
		{
			dokumentenPath = $"Files\\{usedPathname}\\" + path;
			StateHasChanged();
		}
		protected override void OnInitialized()
		{
			base.OnInitialized();
			fileHandler = new FileHandler("", "");
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

			//Folder Löschen
		}


		private void DeleteFile(string filename)
		{
			if (fileHandler is not null)
				fileHandler.DeleteFile(filename);
			File.Delete($"{env.WebRootPath}\\Files\\{usedPathname}\\{filename}");
		}

		private void Refresh()
		{
			navigationManager.NavigateTo("/EmailSinglePage", true);
		}

		private void ResetPage()
		{
			fileHandler = new FileHandler("", "");
			_module = null;
			_dropzoneInstance = null;
			messageText = "Keine Fehler";
			Refresh();
		}
	}
}