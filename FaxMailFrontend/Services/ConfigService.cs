namespace FaxMailFrontend.Services
{
	public class ConfigService
	{
		private readonly IConfiguration _configuration;

		public ConfigService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GetSetting(string key)
		{
			try
			{
				if (_configuration[key] is not null)
					return _configuration[key]!;
				else
					return "20";
			}
			catch (Exception ex)
			{
				throw new Exception($"Error getting setting {key}", ex);
			}
		}
	}
}
