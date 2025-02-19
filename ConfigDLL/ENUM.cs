namespace ConfigDLL
{
	/// <summary>
	/// Systemweit benutze ENUMs für technische und fachliche Abwweisung
	/// </summary>
	///Enums Technische Fehler
	public enum ErrorCode : short
	{
		// Technische Fehler
		DataError = -6,       // Fehler bei Datei- und Verzeichnisoperationen
		FilesystemError = -5,       // Fehler bei Datei- und Verzeichnisoperationen
		ConfigError = -4,           // Konfigurationsfehler
		FileConvertError = -3,      // Konvertierungsfehler
		DatabaseError = -2,         // Datenbankfehler
		TechnicalError = -1,        // Anderer technischer Fehler
		Success = 0
	}
	public enum UmgebungsKennung
	{
		Test = 1,
		Production = 2,
	}

	public enum DatenbankTyp
	{
		Oracle = 1,
		SQLServer = 2
	}
}
