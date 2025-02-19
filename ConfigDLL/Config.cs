using LoggerDLL;

namespace ConfigDLL
{
	/// <summary>
	/// Einfach die Datei App.Config einlesen und die Werte überprüfen
	/// </summary>
	public sealed class Config : IConfig
	{
		public UmgebungsKennung Umgebung { get; set; } = UmgebungsKennung.VDI;
		public string ErrorFolder { get; set; } = "";
		public string AussteuerFolder { get; set; } = "";
		public string BackupVerzeichnis { get; set; } = "";
		public string INI_Endung { get; set; } = "";
		public bool VorgangsSteuerungAktiv { get; set; } = false;
		public bool ReportingAktiv { get; set; } = false;
		public bool PLRPNeuAktiv { get; set; } = false;
		public DatenbankTyp ReportingSwitch { get; set; } = DatenbankTyp.SQLServer;
		public DatenbankTyp WIZAbgleichSwitch { get; set; } = DatenbankTyp.SQLServer;


		private static Config? instance = null;
		public static Config GetInstance()
		{
			if (instance == null)
				instance = new Config();
			return instance;
		}

		private readonly List<string> _configKeys = new()
		{
			"Umgebung",
			"ErrorFolder",
			"AussteuerFolder",
			"BackupVerzeichnis",
			"INI_Endung",
			"VorgangsSteuerungAktiv",
			"ReportingAktiv",
			"PLRPNeuAktiv",
			"ReportingSwitch",
			"WIZAbgleichSwitch",
		};

		public Config()
		{
			try
			{
				ReadConfig();
			}
			catch
			{
				throw new Exception("Instanzierung Config fehlgeschlagen.");
			}
		}

		public void ReadConfig()
		{
			foreach (string value in _configKeys)
			{
				string helper = ConfigProvider.GetInstance(Path.Combine(Directory.GetCurrentDirectory(), "App.config")).GetValueFromConfig(value)!;
				if (helper is null)
				{
					GlobalLogProvider.GetInstance().Log.Error($"Die Variable {value} konnten nicht ausgelesen werden.");
					throw new Exception("Null value found in configuration");
				}
				if (!CheckConfig(helper, value))
				{
					throw new Exception("Condition not met in config check");
				}
			}
		}
		public bool CheckConfig(string helper, string name)
		{
			switch (name)
			{
				case "Umgebung":
					if (helper.ToUpper() == "NIFI")
						Umgebung = UmgebungsKennung.NiFi;
					else if (helper.ToUpper() == "VDI")
						Umgebung = UmgebungsKennung.VDI;
					else
					{
						GlobalLogProvider.GetInstance().Log.Error($"Die Umgebung {helper} ist nicht bekannt.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					return true;
				case "ErrorFolder":
					if (!Directory.Exists(helper))
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Ordner {helper} existiert nicht.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					ErrorFolder = helper;
					return true;
				case "AussteuerFolder":
					if (!Directory.Exists(helper))
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Ordner {helper} existiert nicht.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					AussteuerFolder = helper;
					return true;
				case "BackupVerzeichnis":
					if (!Directory.Exists(helper))
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Ordner {helper} existiert nicht.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					BackupVerzeichnis = helper;
					return true;
				case "INI_Endung":
					INI_Endung = helper;
					return true;
				case "VorgangsSteuerungAktiv":
					if (helper.ToUpper() == "JA")
						VorgangsSteuerungAktiv = true;
					else if (helper.ToUpper() == "NEIN")
						VorgangsSteuerungAktiv = false;
					else
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Wert für VorgangsSteuerungAktiv \"{helper}\" ist nicht bekannt.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					return true;
				case "ReportingAktiv":
					if (helper.ToUpper() == "JA")
						ReportingAktiv = true;
					else if (helper.ToUpper() == "NEIN")
						ReportingAktiv = false;
					else
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Wert für ReportingAktiv \"{helper}\" ist nicht bekannt.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					return true;
				case "PLRPNeuAktiv":
					if (helper.ToUpper() == "JA")
						PLRPNeuAktiv = true;
					else if (helper.ToUpper() == "NEIN")
						PLRPNeuAktiv = false;
					else
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Wert für PLRPNeuAktiv \"{helper}\" ist nicht bekannt.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					return true;
				case "ReportingSwitch":
					if (helper.ToUpper() == "SQLSERVER")
						ReportingSwitch = DatenbankTyp.SQLServer;
					else if (helper.ToUpper() == "ORACLE")
						ReportingSwitch = DatenbankTyp.Oracle;
					else
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Wert für ReportingSwitch \"{helper}\" ist nicht bekannt.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					return true;
				case "WIZAbgleichSwitch":
					if (helper.ToUpper() == "SQLSERVER")
						WIZAbgleichSwitch = DatenbankTyp.SQLServer;
					else if (helper.ToUpper() == "ORACLE")
						WIZAbgleichSwitch = DatenbankTyp.Oracle;
					else
					{
						GlobalLogProvider.GetInstance().Log.Error($"Der Wert für WIZAbgleichSwitch \"{helper}\" ist nicht bekannt.");
						Environment.Exit((int)ErrorCode.ConfigError);
					}
					return true;
				default:
					GlobalLogProvider.GetInstance().Log.Error($"Something wierd in the neighborhood");
					return false;
			}
		}
	}
}
