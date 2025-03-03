using DataAccessDLL.Interfaces;
using DataAccessDLL.Modell;

namespace DataAccessDLL.Services
{
	public interface IFavoritenService
	{
		Task<long> CreateNutzerByKennungAndReturnNewID(INutzer nutzer);
		Task<List<INutzer>> GetAllNutzer();
		Task<List<IFavoriten>> GetFavoritenByNutzer(string NutzerKennung);
		Task<long> GetUserIDFromKennung(string NutzerKennung);
		Task<bool> IsFavoritExistingByUser(string NutzerKennung, string Dokumentenklasse);
		Task<bool> IsNutzerExisting(string NutzerKennung);
	}
}