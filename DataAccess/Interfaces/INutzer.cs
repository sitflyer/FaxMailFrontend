using DataAccessDLL.Modell;

namespace DataAccessDLL.Interfaces
{
	public interface INutzer
	{
		string Email { get; set; }
		ICollection<Favoriten> Favoritens { get; set; }
		long Id { get; set; }
		string? Kennung { get; set; }
		string Nachname { get; set; }
		string Telefonnummer { get; set; }
		string Vorname { get; set; }
	}
}