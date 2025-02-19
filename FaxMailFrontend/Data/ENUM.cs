namespace FaxMailFrontend.Data
{
	public enum Ruechweisungsgrund : short
	{
		KeineRueckweisung = 0,
		DateiZuGroß = 1,
		DateiNichtErlaubt = 2,
		PDFMoreThanAllowedPages = 3,
		FileTypeNotSupported = 4,
		PDFPasswordProtected = 5,
		FilenameContainsSpecialCharacters = 6
	}
	public enum ErrorCode : short
	{
		KeinFehler = 0,
		AllgemeinerFehler = 1,
		DateiFehler = 2,
		DatenKonntenNichtKopiertWerden = 3,
		KeineDateiGefunden = 4,
		KeineDateiGeloescht = 5,
		KeineDateiHochgeladen = 6,
		DBFehler = 7,
		ConfigFehler = 8,
		VerzeichnisKonnteNichtAngelegtWerden = 9,
	}
}
