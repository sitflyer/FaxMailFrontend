using DataAccessDLL.Interfaces;

namespace DataAccessDLL.Services
{
	public interface IStammDatenService
	{
		Task<List<IStammdatenVersicherte>> GetAllVersicherte();
		Task<List<IStammdatenVersicherte>> GetVersichertenByBPNR(string bprn);
		Task<List<IStammdatenVersicherte>> GetVersichertenByKVNR10(string kvnr10);
		Task<List<IStammdatenVersicherte>> GetVersichertenByKVNR9(string kvnr9);
		Task<List<IStammdatenVersicherte>> GetVersichertenByRVNR(string rvnr);
	}
}