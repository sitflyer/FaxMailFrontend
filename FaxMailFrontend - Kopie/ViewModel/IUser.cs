namespace FaxMailFrontend.ViewModel
{
	public interface IUser
	{
		string Email { get; set; }
		string Nachname { get; set; }
		string Telefon { get; set; }
		string Vorname { get; set; }
	}
}