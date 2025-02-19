namespace FaxMailFrontend.ViewModel
{
	public class FileInformation
	{
		public int ID { get; set; } = 0;
		public string FileName { get; set; } = string.Empty;
		public byte[] FileData { get; set; } = new byte[0];
		public bool isSaved { get; set; } = false;
	}
}
