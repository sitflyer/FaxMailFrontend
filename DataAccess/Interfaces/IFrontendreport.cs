namespace DataAccessDLL.Interfaces
{
	public interface IFrontendreport
	{
		string? Boid { get; set; }
		string? Bpnr { get; set; }
		string? Btnr { get; set; }
		string? Dokumentenklasse { get; set; }
		string Dokumenttyp { get; set; }
		DateTime EingereichtAm { get; set; }
		string Filesize { get; set; }
		string FinalerName { get; set; }
		long Id { get; set; }
		string? Kanalart { get; set; }
		string? Kvnr { get; set; }
		string Nutzerkennung { get; set; }
		string Originalname { get; set; }
		string? Produktgruppe { get; set; }
	}
}