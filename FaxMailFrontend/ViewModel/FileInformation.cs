using FaxMailFrontend.Data;

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
		public KanalArt KanalArt { get; set; } = KanalArt.unbestimmt;
		public Ordnungsbegriff Ordnugsbegrff = Ordnungsbegriff.unbestimmt;
		public WeitereOrdnungsbegriffe WOB { get; set; } = WeitereOrdnungsbegriffe.unbestimmt;
		public string BTNR { get; set; } = string.Empty;
		public string KVNR { get; set; } = string.Empty;
		public string GPNR { get; set; } = string.Empty;
		public string LEIK { get; set; } = string.Empty;
		public string Fallnummer { get; set; } = string.Empty;
		public string Fallbuendelnummer { get; set; } = string.Empty;
		public string Produktgruppe { get; set; } = string.Empty;
		public bool MailBodyVerakten = true;
		public bool MailBodyApproved = false;
		public bool KanalartApproved { get; set; } = false;
		public bool OrdnungsbegriffAppoved { get; set; } = false;
		public bool SuchergebnisAppoved { get; set; } = false;
		public bool WeitereOB { get; set; } = false;
		public BodyVeraktung MailbodyVerakten { get; set; } = BodyVeraktung.unbestimmt;
		public bool AbsendenApproved { get; set; } = false;
		public bool Autoermittlung { get; set; } = false;
		public Cascade? Cascade { get; set; }
	}
}
