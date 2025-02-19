namespace MailDLL
{
	public enum MailErrorCode : short
	{

		DataError = -6,             // Fehler bei Datenproblemen
		FilesystemError = -5,       // Fehler bei Datei- und Verzeichnisoperationen
		ConfigError = -4,           // Konfigurationsfehler
		FileConvertError = -3,      // Konvertierungsfehler
		DatabaseError = -2,         // Datenbankfehler
		TechnicalError = -1,        // Anderer technischer Fehler
		Success = 0,
		FinalCopyError = 1,
	}
	public enum AttachmentType : short
	{
		unbekannt = 0,
		invalid = 1,
		PDF = 2,
		PNG = 3,
		JPG = 4,
		TXT = 5,
		TIFF = 6
	}
}
