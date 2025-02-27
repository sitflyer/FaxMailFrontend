using DataAccessDLL.Interfaces;
using DataAccessDLL.Services;
using iText.Kernel.Pdf;

namespace FaxMailFrontend.ViewModel
{
	public class FileHandler
	{
		public List<FileInformation> Files { get; set; } = new List<FileInformation>();
		public string UUID { get; set; } = "";
		public string Path { get; set; } = string.Empty;
		public string ErrorMessage { get; set; } = string.Empty;
		public int selectedFileIndex = -1;
		public string Targetfolder { get; set; } = string.Empty;
		public string Protokollfolder { get; set; } = string.Empty;
		public List<IDokumentenProcessor> DokumentenListe { get; set; }
		public List<string> GPOSList { get; set; } = new List<string>();
		public long UserID { get; set; } = 1;
		public string Vorname { get; set; } = "";
		public string Name { get; set; } = "";
		public string Email { get; set; } = "";
		public string Telefon { get; set; } = "";

		public FileHandler(IDokuService service, IStammDatenService stammDaten, string vorname, string nachname, string telefone, string email)
		{
			Vorname = vorname;
			Name = nachname;
			Telefon = telefone;
			Email = email;
			try
			{
				DokumentenListe = service.GetAllDocumentsSync();
				GPOSList = stammDaten.GetProduktgruppenDistinctSync();
			}
			catch (Exception ex)
			{
				DokumentenListe = new List<IDokumentenProcessor>();
				ErrorMessage = ex.Message;
			}
		}
		public void SetUser(string vorname, string nachname, string telefone, string email)
		{
			Vorname = vorname;
			Name = nachname;
			Telefon = telefone;
			Email = email;
		}
		public void AddFile(string filename, byte[] filedata, int index)
		{
			Files.Add(new FileInformation
			{
				ID = index,
				FileName = filename,
				FileData = filedata
			});
		}
		internal bool DeleteFile(string filename)
		{
			try
			{
				var F = Files.Where(f => f.FileName == filename).FirstOrDefault();
				if (F is not null)
				{
					string path = System.IO.Path.Combine(Path, UUID, F.FileName);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
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
		public static string? CheckFile(string filename, int maxMB)
		{
			if (!CheckFileSize(filename, maxMB))
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
						const int maxPageCount = 300;
						return numberOfPages <= maxPageCount;
					}
				}
			}
			catch
			{
				return false;
			}
		}
		private static bool CheckFileSize(string filename, int maxMB)
		{
			const long maxFileSize = 20 * 1024 * 1024;
			FileInfo fileInfo = new FileInfo(filename);
			return fileInfo.Length <= maxFileSize;
		}
	}
}
