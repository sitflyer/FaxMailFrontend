namespace DataAccessDLL.Modell
{
	public interface IFavoriten
	{
		string Dokumentenklasse { get; set; }
		DokumentenProcessor DokumentenklasseNavigation { get; set; }
		long Id { get; set; }
		Nutzer Nutzer { get; set; }
		long NutzerId { get; set; }
	}
}