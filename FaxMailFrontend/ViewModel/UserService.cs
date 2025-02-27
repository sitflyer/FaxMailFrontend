namespace FaxMailFrontend.ViewModel
{
	public class UserService : IUserService
	{
		public string Vorname { get; set; }
		public string Nachname { get; set; }
		public string Telefon { get; set; }
		public string Email { get; set; }

		public UserService()
		{
			Vorname = "Cindy";
			Nachname = "Kassab";
			Telefon = "123456789";
			Email = "c.k@text.com";
		}
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
