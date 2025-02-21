
namespace FaxMailFrontend.ViewModel
{
	public interface IFileHandler
	{
		string ErrorMessage { get; set; }
		List<FileInformation> Files { get; set; }
		string Path { get; set; }
		string UUID { get; set; }

		static abstract string? CheckFile(string filename);
		void AddFile(string filename, byte[] filedata);
		void LoadFiles();
	}
}