public class LoginService
{
	private readonly HttpClient _httpClient;

	public LoginService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<LoginInfo> GetLoginInfoAsync(string apiUrl)
	{
		var response = await _httpClient.GetAsync(apiUrl);
		response.EnsureSuccessStatusCode();

		var loginInfo = await response.Content.ReadFromJsonAsync<LoginInfo>();
		return loginInfo;
	}
}

public class LoginInfo
{
	public string Username { get; set; }
	public string Token { get; set; }
	// Weitere Anmeldeinformationen
}