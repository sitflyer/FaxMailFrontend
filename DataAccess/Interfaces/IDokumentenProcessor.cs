using DataAccessDLL.Modell;

namespace DataAccessDLL.Interfaces
{
	public interface IDokumentenProcessor
	{
		string Dokumentart { get; set; }
		string Dokumentklasse { get; set; }
		string Dokumenttyp { get; set; }
		ICollection<Favoriten> Favoritens { get; set; }
		byte Kanalart { get; set; }
		string Kategorie { get; set; }
		byte Mailbody { get; set; }
		byte Ob { get; set; }
		byte OtherOb { get; set; }
		string Unterkategorie { get; set; }
	}
}