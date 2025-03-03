using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FaxMailFrontend.ViewModel
{
	public class UserService : IUserService
	{


		private readonly HttpClient _httpClient;

		public UserService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IUser> GetUserAsync()
		{
			//var user = await _httpClient.GetFromJsonAsync<User>("https://api.entraid.com/user");
			//if (user != null)
			//{
			//	Vorname = "Oliver";
			//	Nachname = "Perschke";
			//	Telefon = "0171778121";
			//	Email = "o@p.de";
			//}
			User user = new User();
			user.Vorname = "Oliver";
			user.Nachname = "Perschke";
			user.Telefon = "0171778121";
			user.Email = "o@p.de";
			return user;
		}
	}

	public class User : IUser
	{
		public string Vorname { get; set; }
		public string Nachname { get; set; }
		public string Telefon { get; set; }
		public string Email { get; set; }
	}
}
