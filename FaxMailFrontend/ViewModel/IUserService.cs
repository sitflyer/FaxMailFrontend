
namespace FaxMailFrontend.ViewModel
{
	public interface IUserService
	{
		string GetUser();
		Task<string> GetUserAsync();
	}
}