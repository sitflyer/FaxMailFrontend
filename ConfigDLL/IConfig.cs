namespace ConfigDLL
{
	public interface IConfig
	{
		string AussteuerFolder { get; set; }
		string BackupVerzeichnis { get; set; }
		string ErrorFolder { get; set; }
		string INI_Endung { get; set; }
		bool PLRPNeuAktiv { get; set; }
		bool ReportingAktiv { get; set; }
		DatenbankTyp ReportingSwitch { get; set; }
		UmgebungsKennung Umgebung { get; set; }
		bool VorgangsSteuerungAktiv { get; set; }
		DatenbankTyp WIZAbgleichSwitch { get; set; }

		bool CheckConfig(string helper, string name);
		void ReadConfig();
	}
}