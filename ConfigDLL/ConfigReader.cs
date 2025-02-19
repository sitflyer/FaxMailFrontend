using LoggerDLL;
using System.Configuration;

namespace ConfigDLL
{
	public class ConfigReader
	{

		private static Configuration? _config;
		public bool Valid { get; private set; } = false;
		public bool FoundEmptyEntries { get; private set; } = false;
		/// <summary>
		/// Konstruktur dem die Konfigurationsdatei übergeben werden muss
		/// </summary>
		/// <param name="configFile"></param>
		public ConfigReader(string configFile)
		{
			Configuration config = GetConfig(configFile)!;
			if (config != null)
			{
				_config = config;
				Valid = true;
			}
		}
		/// <summary>
		/// Stellt die Basis für die Werteermittlung dar und spezifiziert das Konfigfile
		/// </summary>
		/// <param name="dllLocation"></param>
		/// <returns></returns>
		private static Configuration? GetConfig(string dllLocation)
		{
			if (!File.Exists(dllLocation))
			{
				GlobalLogProvider.GetInstance(GlobalLogProvider.LoggerConfigFile!).Log.Debug($"Die Config Datei {dllLocation} existiert nicht.");
				return null;
			}
			ExeConfigurationFileMap fileMap = new();
			fileMap.ExeConfigFilename = dllLocation;
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			return config;
		}
		/// <summary>
		/// Liest einen Wert auf Basis eines Schlüssels aus
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetConfigProperty(string key)
		{
			FoundEmptyEntries = false;
			string retvalue;
			try
			{
				retvalue = _config!.AppSettings.Settings[key].Value;
			}
			catch
			{
				retvalue = string.Empty;
				FoundEmptyEntries = true;
			}
			return retvalue;
		}
		/// <summary>
		/// Liest eine Wertegruppe auf Basis der Schlüsselliste aus
		/// </summary>
		/// <param name="keyList"></param>
		/// <returns></returns>
		public List<string>? GetConfigList(List<string> keyList)
		{
			List<string> values = new() { };
			foreach (string key in keyList)
			{
				string localValue = GetConfigProperty(key);
				if (FoundEmptyEntries)
				{
					return null;
				}
				values.Add(localValue);
			}
			return values;
		}
		public static bool GetBoolValue(string helper)
		{
			if (helper == "Ja")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool IsBool(string helper, string name)
		{
			if (helper == "Ja" || helper == "Nein")
			{
				return true;
			}
			GlobalLogProvider.GetInstance().Log.Error($"Die Variable {name} konnte nicht in einen Bool konvertiert werden.");
			return false;
		}
		public static bool IsInt(string helper, string name)
		{
			try
			{
				long x = Int64.Parse(helper);
			}
			catch
			{
				GlobalLogProvider.GetInstance().Log.Error($"Die Variable {name} konnte nicht in einen Integer konvertiert werden.");
				return false;
			}
			return true;
		}
		public static bool IsExistingDirectory(string helper, string name)
		{
			if (!Directory.Exists(helper))
			{
				GlobalLogProvider.GetInstance().Log.Error($"Die Variable {name} ist kein existierendes Verzeichnis.");
				return false;
			}
			return true;
		}
		public static bool IsExistingFile(string helper, string name)
		{
			if (!File.Exists(helper))
			{
				GlobalLogProvider.GetInstance().Log.Error($"Die Variable {name} ist keine existierende Datei.");
				return false;
			}
			return true;
		}
	}
}

