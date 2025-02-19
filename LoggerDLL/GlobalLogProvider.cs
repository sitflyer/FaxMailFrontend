namespace LoggerDLL
{
	public class GlobalLogProvider
	{
		private static GlobalLogProvider? _log;
		public static string? LoggerConfigFile { get; set; }
		//private static string _configFile;
		public log4net.ILog Log { get; private set; }

		/// <summary>
		/// Konstruktor der Loggers aus Defaultdatei
		/// </summary>
		private GlobalLogProvider()
		{
			try
			{
				string loggerConfigFile = Directory.GetCurrentDirectory() + "\\log4net.config";
				if (!File.Exists(loggerConfigFile))
				{
					Console.WriteLine($"Das Konfigurationsfile für den Logger \"{loggerConfigFile}\" wurde nicht gefunden.");
					Environment.Exit(-1);
				}
				LoggerConfigFile = loggerConfigFile;
				log4net.Config.XmlConfigurator.Configure(new FileInfo(loggerConfigFile));
				var declaringType = System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType;
				if (declaringType == null)
				{
					throw new InvalidOperationException("Declaring type is null");
				}
				Log = log4net.LogManager.GetLogger(declaringType);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fehler in der Logger initialisierung");
				Console.WriteLine($"Systemmeldung => {ex.Message}");
				Environment.Exit(-1);
			}
		}
		/// <summary>
		/// Konstruktur des Loggers mit Dateinameübergabe
		/// </summary>
		/// <param name="configFileName"></param>
		private GlobalLogProvider(string configFileName)
		{
			try
			{
				if (!File.Exists(configFileName))
				{
					Console.WriteLine($"Das Konfigurationsfile für den Logger \"{configFileName}\" wurde nicht gefunden.");
					Environment.Exit(-1);
				}
				log4net.Config.XmlConfigurator.Configure(new FileInfo(configFileName));
				var declaringType = System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType;
				if (declaringType == null)
				{
					throw new InvalidOperationException("Declaring type is null");
				}
				Log = log4net.LogManager.GetLogger(declaringType);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Fehler in der Logger initialisierung");
				Console.WriteLine($"Systemmeldung => {ex.Message}");
				Environment.Exit(-1);
			}
		}

		/// <summary>
		/// Singleton für den Logger aus Standarddatei
		/// </summary>
		/// <returns></returns>
		public static GlobalLogProvider GetInstance()
		{
			{
				_log ??= new GlobalLogProvider();
				return _log;
			}
		}
		/// <summary>
		/// Singleton für den Logger aus beliebiger Datei
		/// </summary>
		/// <param name="_configFile"></param>
		/// <returns></returns>
		public static GlobalLogProvider GetInstance(string configFileName)
		{
			{
				_log ??= new GlobalLogProvider(configFileName);
				return _log;
			}
		}
	}
}
