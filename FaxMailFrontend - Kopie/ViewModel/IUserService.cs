
namespace FaxMailFrontend.ViewModel
{
	public interface IUserService
	{
		Task<IUser> GetUserAsync();
	}
}