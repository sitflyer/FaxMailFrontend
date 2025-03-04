namespace DataAccessDLL.Interfaces
{
	public interface IStammDatenService
	{
		Task<List<IStammdatenVersicherte>> GetAllVersicherte();
		List<IStammdatenVersicherte> GetAllVersicherteSync();
		Task<List<IGposListe>> GetGPosliste();
		List<IGposListe> GetGPoslisteSync();
		Task<List<string>> GetProduktgruppenDistinct();
		List<string> GetProduktgruppenDistinctSync();
		Task<List<IStammdatenVersicherte>> GetVersichertenByBPNR(string bpnr);
		List<IStammdatenVersicherte> GetVersichertenByBPNRSync(string bpnr);
		Task<List<IStammdatenFirmenkunde>> GetVersichertenByBTNR(string btnr);
		List<IStammdatenFirmenkunde> GetVersichertenByBTNRSync(string btnr);
		Task<List<IStammdatenVersicherte>> GetVersichertenByKVNR10(string kvnr10);
		List<IStammdatenVersicherte> GetVersichertenByKVNR10Sync(string kvnr10);
		Task<List<IStammdatenVersicherte>> GetVersichertenByKVNR9(string kvnr9);
		List<IStammdatenVersicherte> GetVersichertenByKVNR9Sync(string kvnr9);
		Task<List<ILeistungserbringerLanr>> GetVersichertenByLeik(string bpnr);
		List<ILeistungserbringerLanr> GetVersichertenByLeikSync(string bpnr);
	}
}