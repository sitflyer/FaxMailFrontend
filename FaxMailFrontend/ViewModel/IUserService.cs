
namespace FaxMailFrontend.ViewModel
{
	public interface IUserService
	{
		string Email { get; set; }
		string Nachname { get; set; }
		string Telefon { get; set; }
		string Vorname { get; set; }

		string GetUser();
		Task<string> GetUserAsync();
	}
}