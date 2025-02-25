using DataAccessDLL.Interfaces;
namespace DataAccessDLL.Modell;

public partial class StammdatenFirmenkunde : IStammdatenFirmenkunde
{
	public string? Btnr { get; set; }

	public string? Identart { get; set; }

	public string? Firmenname { get; set; }

	public string? Plz { get; set; }

	public string? Ort { get; set; }

	public string? Strasse { get; set; }

	public string? Hausnummer { get; set; }

	public int UniqueId { get; set; }

	public string? Verarbdatum { get; set; }
}
