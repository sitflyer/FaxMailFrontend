using DataAccessDLL.Interfaces;

namespace DataAccessDLL.Modell;

public partial class StammdatenVersicherte : IStammdatenVersicherte
{
	public string? Kvnr10 { get; set; }

	public string? Kvnr9 { get; set; }

	public string? Name { get; set; }

	public string? Vorname { get; set; }

	public string? Geburtsdatum { get; set; }

	public string? Geschlecht { get; set; }

	public string? Plz { get; set; }

	public string? Ort { get; set; }

	public string? Strasse { get; set; }

	public string? KzMitarbeiter { get; set; }

	public string? Rvnr { get; set; }

	public string Bpnr { get; set; } = null!;

	public string? Blocklist { get; set; }

	public string? Verarbdatum { get; set; }
}
