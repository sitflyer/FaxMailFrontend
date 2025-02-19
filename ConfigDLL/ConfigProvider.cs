using LoggerDLL;

namespace ConfigDLL
{
	public sealed class ConfigProvider
	{
		private static ConfigProvider? _config;
		private readonly ConfigReader _configReader;
		public static string KonfiguratorName { get; set; } = "Master";

		/// <summary>
		/// Erstellt eine Konfiguration aus dem Standardfile App.config unter der Konfigurationsordner
		/// Der Inhalt dieser Datei muss den Standardanforderungen an XML Daten entsprechen
		/// </summary>
		private ConfigProvider()
		{
			try
			{
				string configFileName = Directory.GetCurrentDirectory() + "\\App.config";
				if (!File.Exists(configFileName))
				{
					Console.WriteLine($"Das Konfigurationsfile für den Konfigurator \"{configFileName}\" wurde nicht gefunden.");
					Environment.Exit(-1);
				}
				_configReader = new(configFileName);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fehler in der Konfigurationsinitialisierung");
				Console.WriteLine($"Systemmeldung => {ex.Message}");
				Environment.Exit(-1);
			}
		}

		/// <summary>
		/// Erstellt eine Konfiguration aus einem beliebigen Konfigurationsfile
		/// Der Inhalt dieser Datei muss den Standardanforderungen an XML Daten entsprechen
		/// </summary>
		/// <param name="configFileName"></param>
		private ConfigProvider(string configFileName)
		{
			if (configFileName == string.Empty)
			{
				configFileName = Directory.GetCurrentDirectory() + "\\Konfiguration\\App.config";
			}
			try
			{
				if (!File.Exists(configFileName))
				{
					Console.WriteLine($"Das Konfigurationsfile für den Logger \"{configFileName}\" wurde nicht gefunden.");
					Environment.Exit(-1);
				}
				_configReader = new(configFileName);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fehler in der Konfigurationsinitialisierung");
				Console.WriteLine($"Systemmeldung => {ex.Message}");
				Environment.Exit(-1);
			}
		}
		private ConfigProvider(string configFileName, log4net.ILog log)
		{
			if (configFileName == string.Empty)
			{
				configFileName = Directory.GetCurrentDirectory() + "\\Konfiguration\\App.config";
			}
			try
			{
				if (!File.Exists(configFileName))
				{
					log.Error($"Das Konfigurationsfile für den Logger \"{configFileName}\" wurde nicht gefunden.");
					Environment.Exit(-1);
				}
				_configReader = new(configFileName);
			}
			catch (Exception ex)
			{
				log.Error("Fehler in der Konfigurationsinitialisierung");
				log.Error($"Systemmeldung => {ex.Message}");
				Environment.Exit(-1);
			}
		}
		/// <summary>
		/// Implementierung einses Singletons aus Standardfile
		/// </summary>
		/// <returns></returns>
		public static ConfigProvider GetInstance()
		{
			{
				if (_config == null)
				{
					_config = new ConfigProvider();
				}
				return _config;
			}
		}
		/// <summary>
		/// Implementierung einses Singletons eines beliebigen Files
		/// </summary>
		/// <returns></returns>
		public static ConfigProvider GetInstance(string configFileName)
		{
			if (_config == null)
			{
				_config = new ConfigProvider(configFileName);
			}
			if (GlobalLogProvider.LoggerConfigFile != configFileName)
			{
				_config = new ConfigProvider(configFileName);
			}
			return _config;
		}
		/// <summary>
		/// Implementierung einses Singletons eines beliebigen Files mit Logger
		/// </summary>
		/// <param name="configFileName"></param>
		/// <param name="log"></param>
		/// <returns></returns>
		public static ConfigProvider GetInstance(string configFileName, log4net.ILog log)
		{
			if (_config == null)
			{
				_config = new ConfigProvider(configFileName, log);
			}
			if (GlobalLogProvider.LoggerConfigFile != configFileName)
			{
				_config = new ConfigProvider(configFileName, log);
			}
			return _config;
		}
		/// <summary>
		/// Sammelt alle angeforderten Konfigurationsdaten aus der Konfdatei mittels einer Keylist
		/// </summary>
		/// <param name="keyList"></param>
		/// <returns></returns>
		public List<string>? GetDataListFromConfig(List<string> keyList)
		{
			if (!_configReader.Valid)
			{
				GlobalLogProvider.GetInstance(GlobalLogProvider.LoggerConfigFile!).Log.Debug("Die Konfigdatei konnte nicht korrekt ausgelesen werden");
				return null;
			}
			List<string>? localValueList = _configReader.GetConfigList(keyList);
			if (_configReader.FoundEmptyEntries)
			{
				GlobalLogProvider.GetInstance(GlobalLogProvider.LoggerConfigFile!).Log.Debug("Leere oder fehlende Einträge in Konfiguration gefunden");
				return null;
			}
			return localValueList;
		}
		/// <summary>
		/// Sammlet einen Wert aus der Konfigdatei basierend auf dem übergebenen Schlüssel
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string? GetValueFromConfig(string key)
		{
			if (!_configReader.Valid)
			{
				GlobalLogProvider.GetInstance(GlobalLogProvider.LoggerConfigFile!).Log.Debug("Die Konfigdatei konnte nicht korrekt ausgelesen werden");
				return null;
			}
			List<string>? localValueList = _configReader.GetConfigList(new() { key });
			if (_configReader.FoundEmptyEntries)
			{
				GlobalLogProvider.GetInstance(GlobalLogProvider.LoggerConfigFile!).Log.Debug("Leere oder fehlende Einträge in Konfiguration gefunden");
				return null;
			}
			return localValueList![0];
		}
	}
}
