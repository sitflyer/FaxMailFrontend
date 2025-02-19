
namespace FaxMailFrontend.ViewModel
{
	public interface IFileHandler
	{
		string ErrorMessage { get; set; }
		List<FileInformation> Files { get; set; }
		string Path { get; set; }
		string UUID { get; set; }

		void LoadFiles();
	}
}