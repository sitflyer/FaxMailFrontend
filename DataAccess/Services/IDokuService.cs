using DataAccessDLL.Interfaces;

namespace DataAccessDLL.Services
{
	public interface IDokuService
	{
		Task<List<IDokumentenProcessor>> GetAllDocuments();
		Task<List<IDokumentenProcessor>> GetAllDocumentsByKategorie(string kategorie);
		List<IDokumentenProcessor> GetAllDocumentsByKategorieSync(string kategorie);
		List<IDokumentenProcessor> GetAllDocumentsSync();
		Task<List<string>> GettAllDocumentsBySubkategory(string subkategorie);
		Task<List<string>> GettAllDocumentsBySubkategory(string subkategorie, string category);
		List<string> GettAllDocumentsBySubkategorySync(string subkategorie);
		List<string> GettAllDocumentsBySubkategorySync(string subkategorie, string category);
		Task<List<IDokumentenProcessor>> GettAllDocumentsByUnterkategorie(string unterkategorie);
		List<IDokumentenProcessor> GettAllDocumentsByUnterkategorieSync(string unterkategorie);
		Task<List<string>> GettAllKategories();
		List<string> GettAllKategoriesSync();
		Task<List<string>> GettAllSubKategoriesByKategory(string kategorie);
		List<string> GettAllSubKategoriesByKategorySync(string kategorie);
		Task<IDokumentenProcessor> GettDocuProByName(string dokuklasse);
		IDokumentenProcessor GettDocuProByNameSync(string dokuklasse);
	}
}