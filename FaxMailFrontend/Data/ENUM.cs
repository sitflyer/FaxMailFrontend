namespace FaxMailFrontend.Data
{
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

	public enum KanalArt : short
	{
		unbestimmt = 0,
		EPost = 1,
		lateScan = 2,
		Beide = 3,
		Fehlerhaft = 4,
	}
	public enum BodyVeraktung : short
	{
		unbestimmt = 0,
		Ja = 1,
		Nein = 2,
		Fehlerhaft = 3,
	}


}
