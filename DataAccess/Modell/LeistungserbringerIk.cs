using DataAccessDLL.Interfaces;
namespace DataAccessDLL.Modell;

public partial class LeistungserbringerIk : ILeistungserbringerIk
{
	public string Bpnr { get; set; } = null!;

	public string? Type { get; set; }

	public string? Rolle { get; set; }

	public string? Bezeichnung { get; set; }

	public string? Betriebsstaette { get; set; }

	public string? Name1 { get; set; }

	public string? Name2 { get; set; }

	public string? Name3 { get; set; }

	public string? Name4 { get; set; }

	public string? Plz { get; set; }

	public string? Ort { get; set; }

	public string? Strasse { get; set; }

	public string? Hausnummer { get; set; }

	public string? Verarbdatum { get; set; }
}
