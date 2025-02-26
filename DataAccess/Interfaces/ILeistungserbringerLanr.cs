namespace DataAccessDLL.Interfaces
{
	public interface ILeistungserbringerLanr
	{
		string? Arztnummer { get; set; }
		string? Bezeichnung { get; set; }
		string? Bpnr { get; set; }
		string? Hausnummer { get; set; }
		string? Nachname { get; set; }
		string? Name1 { get; set; }
		string? Name2 { get; set; }
		string? Name3 { get; set; }
		string? Name4 { get; set; }
		string? Ort { get; set; }
		string? Plz { get; set; }
		int Record { get; set; }
		string? Rolle { get; set; }
		string? Strasse { get; set; }
		string? Type { get; set; }
		string? Verarbdatum { get; set; }
		string? Vorname { get; set; }
	}
}