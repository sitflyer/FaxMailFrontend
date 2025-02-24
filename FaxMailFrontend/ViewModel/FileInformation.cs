namespace FaxMailFrontend.ViewModel
{
	public class FileInformation
	{
		public int ID { get; set; } = 0;
		public string FileName { get; set; } = string.Empty;
		public byte[] FileData { get; set; } = new byte[0];
		public bool isSaved { get; set; } = false;
		public bool isEdit { get; set; } = true;
		public string DokumentenKlasse { get; set; } = string.Empty;
		public string KanalArt { get; set; } = string.Empty;
		public string BTNR { get; set; } = string.Empty;
		public string KVNR { get; set; } = string.Empty;
		public string GPNR { get; set; } = string.Empty;
		public string Fallnummer { get; set; } = string.Empty;
		public string Fallbuendelnummer { get; set; } = string.Empty;
		public bool MailBodyVerakten = true;
		public bool KanalartApproved { get; set; } = false;
		public bool OrdnungsbegriffAppoved { get; set; } = false;
		public bool SuchergebnisAppoved { get; set; } = false;
		public bool WeitereOB { get; set; } = false;
		public bool Mailbody { get; set; } = false;
		public bool AbsendenApproved { get; set; } = false;
		public bool Autoermittlung { get; set; } = false;
	}
}
