namespace FaxMailFrontend.ViewModel
{
	public class UserService : IUserService
	{
		public string GetUser()
		{
			return "Cindy Kassab";
		}
		public async Task<string> GetUserAsync()
		{
			await Task.Delay(10);
			return "Cindy Kassab";
		}
	}
}
