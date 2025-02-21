namespace DataAccessDLL.Interfaces
{
	public interface IStammdatenVersicherte
	{
		string? Blocklist { get; set; }
		string Bpnr { get; set; }
		string? Geburtsdatum { get; set; }
		string? Geschlecht { get; set; }
		string? Kvnr10 { get; set; }
		string? Kvnr9 { get; set; }
		string? KzMitarbeiter { get; set; }
		string? Name { get; set; }
		string? Ort { get; set; }
		string? Plz { get; set; }
		string? Rvnr { get; set; }
		string? Strasse { get; set; }
		string? Verarbdatum { get; set; }
		string? Vorname { get; set; }
	}
}