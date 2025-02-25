using iText.Kernel.Pdf;
using DataAccessDLL.Services;
using DataAccessDLL.Interfaces;
using FaxMailFrontend.Data;

namespace FaxMailFrontend.ViewModel
{
	public class FileHandler : IFileHandler
	{
		public List<FileInformation> Files { get; set; } = new List<FileInformation>();
		public string UUID { get; set; } = "";
		public string Path { get; set; } = string.Empty;
		public string ErrorMessage { get; set; } = string.Empty;
		private int filecounter = 0;
		public int selectedFileIndex = -1;
		public string Targetfolder { get; set; } = string.Empty;
		public string Protokollfolder { get; set; } = string.Empty;

		public List<IDokumentenProcessor> DokumentenListe { get; set; }

		public FileHandler(IDokuService service)
		{
			try
			{
				DokumentenListe = service.GetAllDocumentsSync();
			}
			catch (Exception ex)
			{
				DokumentenListe = new List<IDokumentenProcessor>();
				ErrorMessage = ex.Message;
			}
		}

		//public FileHandler(string basepath, string UUID)
		//{
		//	Path = basepath;
		//	this.UUID = UUID;
		//	LoadFiles();
		//}

		public void AddFile(string filename, byte[] filedata, int index)
		{
			Files.Add(new FileInformation
			{
				ID = index,
				FileName = filename,
				FileData = filedata
			});
		}

		//public void LoadFiles()
		//{
		//	try
		//	{
		//		Files.Clear();
		//		if (Directory.Exists(Path))
		//		{
		//			var files = Directory.GetFiles(Path);
		//			int id = 1;
		//			foreach (var file in files)
		//			{
		//				Files.Add(new FileInformation
		//				{
		//					ID = id,
		//					FileName = System.IO.Path.GetFileName(file),
		//					FileData = File.ReadAllBytes(file)
		//				});
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		ErrorMessage = ex.Message;
		//		throw;
		//	}
		//}

		internal bool DeleteFile(string filename)
		{
			try
			{
				var F = Files.Where(f => f.FileName == filename).FirstOrDefault();
				if (F is not null)
					if (File.Exists(Path + F.FileName))
						File.Delete(Path + F.FileName);
				Files.RemoveAll(f => f.FileName == filename);
				foreach (var file in Files)
				{
					file.ID = Files.IndexOf(file);
					selectedFileIndex = -1;
	}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string? CheckFile(string filename)
		{
			if (!CheckFileSize(filename))
				return $"Die Dateigröße des von Ihnen hochgeladenen Dokuments \"{System.IO.Path.GetFileName(filename)}\" ist nicht zulässig. Das Dokument kann nicht verarbeitet werden.";
			if (!CheckType(filename))
				return $"Das von Ihnen hochgeladene Dokument \"{System.IO.Path.GetFileName(filename)}\" stellt kein zulässiges Format dar. Das Dokument kann nicht verarbeitet werden.";
			if (!CheckSpecialChar(filename))
				return $"Die Datei {filename} enthält Sonderzeichen";
			if (System.IO.Path.GetExtension(filename).ToLower() == ".pdf")
			{
				if (!CheckPassword(filename))
					return $"Die Datei {filename} ist passwordgeschützt oder korrupt";
				if (!CheckPageCount(filename))
					return $"„Die erlaubte Seitenanzahl des von Ihnen hochgeladenen Dokuments ist nicht zulässig. Das Dokument kann nicht verarbeitet werden.";
			}
			return null;
		}

		private static bool CheckSpecialChar(string filename)
		{
			string specialChars = @"(!@#$%^&*()-_=+\|[]{};:/?.>)";
			return filename.Any(c => specialChars.Contains(c));
		}

		private static bool CheckPassword(string filename)
		{
			try
			{
				using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(filename)))
				{
					return !pdfDocument.GetReader().IsEncrypted();
				}
			}
			catch
			{
				return false;
			}
		}

		private static bool CheckType(string filename)
		{
			string[] allowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".txt", ".text" };
			string fileExtension = System.IO.Path.GetExtension(filename).ToLower();
			return allowedExtensions.Contains(fileExtension);
		}

		private static bool CheckPageCount(string filename)
		{
			try
			{
				using (PdfReader pdfReader = new PdfReader(filename))
				{
					using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
					{
						int numberOfPages = pdfDocument.GetNumberOfPages();
						const int maxPageCount = 300; // Example max page count
						return numberOfPages <= maxPageCount;
					}
				}
			}
			catch
			{
				return false;
			}
		}

		private static bool CheckFileSize(string filename)
		{
			const long maxFileSize = 20 * 1024 * 1024; // 20 MB in bytes
			FileInfo fileInfo = new FileInfo(filename);
			return fileInfo.Length <= maxFileSize;
		}
	}
}
