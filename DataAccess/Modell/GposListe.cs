using DataAccessDLL.Interfaces;

namespace DataAccessDLL.Modell;

public partial class GposListe : IGposListe
{
	public string Produktgruppe { get; set; } = null!;

	public string BezeichnungGebührenposition { get; set; } = null!;

	public int Id { get; set; }
}
