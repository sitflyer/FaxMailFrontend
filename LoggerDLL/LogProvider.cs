namespace LoggerDLL
{
	public sealed class LogProvider
	{
		public log4net.ILog Log { get; private set; }
		/// <summary>  
		/// Initialisiert das Logging und Wrapped das Log  
		/// </summary>  
		/// <param name="configFileName">Dies ist das Konfigfile für den Logger</param>  
		public LogProvider(string configFileName)
		{
			try
			{
				if (!File.Exists(configFileName))
				{
					Console.WriteLine($"Das Konfigurationsfile für den Logger \"{configFileName}\" wurde nicht gefunden.");
					Environment.Exit(-1);
				}
				log4net.Config.XmlConfigurator.Configure(new FileInfo(configFileName));
				var declaringType = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType;
				if (declaringType is null)
				{
					Console.WriteLine("Logger konnte nicht initialisiert werden.");
					Environment.Exit(-1);
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
		/// Beendet das Logging und gibt das File wieder frei  
		/// </summary>  
		public void TerminateLogging()
		{
			if (Log is not null)
			{
				log4net.LogManager.Shutdown();
			}
		}
	}
}
