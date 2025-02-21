using DataAccessDLL.Interfaces;

namespace DataAccessDLL.Services
{
	public interface IDokuService
	{
		Task<List<IDokumentenProcessor>> GetAllDocuments();
		Task<List<IDokumentenProcessor>> GetAllDocumentsByKategorie(string kategorie);
		Task<List<string>> GettAllDocumentsBySubkategory(string subkategorie);
		Task<List<string>> GettAllDocumentsBySubkategory(string subkategorie, string category);
		Task<List<IDokumentenProcessor>> GettAllDocumentsByUnterkategorie(string unterkategorie);
		Task<List<string>> GettAllKategories();
		Task<List<string>> GettAllSubKategoriesByKategory(string kategorie);
		Task<IDokumentenProcessor> GettDocuProByName(string dokuklasse);
	}
}