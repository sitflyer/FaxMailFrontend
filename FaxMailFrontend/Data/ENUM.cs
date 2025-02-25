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
	public enum Ordnungsbegriff : short
	{
		unbestimmt = 0,
		KVNR = 1,
		BTNR = 2,
		GPNR = 3,
		Leik = 4,
		Keiner = 5
	}
	public enum WeitereOrdnungsbegriffe : short
	{
		unbestimmt = 0,
		optFallnrFallbuendellNr = 1,
		PfichtProduktgruppe = 2,
		OptProduktgruppe = 3,
		Keiner = 4
	}

}
