using DataAccessDLL.Interfaces;
using DataAccessDLL.Modell;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace DataAccessDLL.Services
{
	public class DokuService : IDokuService
	{
		private IWorkingContext WorkingContext;
		public DokuService(IWorkingContext mycontext)
		{
			WorkingContext = mycontext;
		}
		public async Task<List<IDokumentenProcessor>> GetAllDocuments()
		{
			try
			{
				return await WorkingContext.DokumentenProcessors.Cast<IDokumentenProcessor>().ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public List<IDokumentenProcessor> GetAllDocumentsFromFavoritenSync()
		{
			try
			{
				return WorkingContext.DokumentenProcessors.Cast<IDokumentenProcessor>().ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}
		public List<IDokumentenProcessor> GetAllDocumentsSync()
		{
			try
			{
				return WorkingContext.DokumentenProcessors.Cast<IDokumentenProcessor>().ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public async Task<List<IDokumentenProcessor>> GetAllDocumentsByKategorie(string kategorie)
		{
			try
			{
				return await WorkingContext.DokumentenProcessors
					.Where(dp => dp.Kategorie == kategorie)
					.Cast<IDokumentenProcessor>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public List<IDokumentenProcessor> GetAllDocumentsByKategorieSync(string kategorie)
		{
			try
			{
				return WorkingContext.DokumentenProcessors
					.Where(dp => dp.Kategorie == kategorie)
					.Cast<IDokumentenProcessor>()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public async Task<List<IDokumentenProcessor>> GettAllDocumentsByUnterkategorie(string unterkategorie)
		{
			try
			{
				return await WorkingContext.DokumentenProcessors
					.Where(dp => dp.Unterkategorie == unterkategorie)
					.Cast<IDokumentenProcessor>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public List<IDokumentenProcessor> GettAllDocumentsByUnterkategorieSync(string unterkategorie)
		{
			try
			{
				return WorkingContext.DokumentenProcessors
					.Where(dp => dp.Unterkategorie == unterkategorie)
					.Cast<IDokumentenProcessor>()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public async Task<List<string>> GettAllKategories()
		{
			try
			{
				return await WorkingContext.DokumentenProcessors
					.Select(dp => dp.Kategorie)
					.Distinct()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public List<string> GettAllKategoriesSync()
		{
			try
			{
				return WorkingContext.DokumentenProcessors
					.Select(dp => dp.Kategorie)
					.Distinct()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public async Task<List<string>> GettAllSubKategoriesByKategory(string kategorie)
		{
			try
			{
				return await WorkingContext.DokumentenProcessors
					.Where(dp => dp.Kategorie == kategorie)
					.Select(dp => dp.Unterkategorie)
					.Distinct()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public List<string> GettAllSubKategoriesByKategorySync(string kategorie)
		{
			try
			{
				return WorkingContext.DokumentenProcessors
					.Where(dp => dp.Kategorie == kategorie)
					.Select(dp => dp.Unterkategorie)
					.Distinct()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public async Task<List<string>> GettAllDocumentsBySubkategory(string subkategorie)
		{
			try
			{
				return await WorkingContext.DokumentenProcessors
					.Where(dp => dp.Unterkategorie == subkategorie)
					.Select(dp => dp.Dokumentklasse)
					.Distinct()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public List<string> GettAllDocumentsBySubkategorySync(string subkategorie)
		{
			try
			{
				return WorkingContext.DokumentenProcessors
					.Where(dp => dp.Unterkategorie == subkategorie)
					.Select(dp => dp.Dokumentklasse)
					.Distinct()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public async Task<IDokumentenProcessor> GettDocuProByName(string dokuklasse)
		{
			try
			{
				return await WorkingContext.DokumentenProcessors
					.Where(dp => dp.Dokumentklasse == dokuklasse)
					.FirstAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public IDokumentenProcessor GettDocuProByNameSync(string dokuklasse)
		{
			try
			{
				return WorkingContext.DokumentenProcessors
					.Where(dp => dp.Dokumentklasse == dokuklasse)
					.First();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public async Task<List<string>> GettAllDocumentsBySubkategory(string subkategorie, string category)
		{
			try
			{
				return await WorkingContext.DokumentenProcessors
					.Where(dp => dp.Unterkategorie == subkategorie && dp.Kategorie == category)
					.Select(dp => dp.Dokumentklasse)
					.Distinct()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}

		public List<string> GettAllDocumentsBySubkategorySync(string subkategorie, string category)
		{
			try
			{
				return WorkingContext.DokumentenProcessors
					.Where(dp => dp.Unterkategorie == subkategorie && dp.Kategorie == category)
					.Select(dp => dp.Dokumentklasse)
					.Distinct()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Dokumentenprozessoren: " + ex.Message);
			}
		}
		public List<string> GettAllFavoritesByUserSync(long userID)
		{
			try
			{
				return WorkingContext.Favoritens
					.Where(f => f.NutzerId == userID)
					.Select(f => f.Dokumentenklasse)
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Favoriten: " + ex.Message);
			}
		}
		public async Task<List<string>> GettAllFavoritesByUser(long userID)
		{
			try
			{
				return await WorkingContext.Favoritens
					.Where(f => f.NutzerId == userID)
					.Select(f => f.Dokumentenklasse)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Favoriten: " + ex.Message);
			}
		}
		public async Task AddFavorite(long id, string dokklasse)
		{
			try
			{
				Favoriten fav = new Favoriten();
				fav.NutzerId = id;
				fav.Dokumentenklasse = dokklasse;
				WorkingContext.Favoritens.Add(fav);
				await WorkingContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Hinzufügen des Favoriten: " + ex.Message);
			}
		}
		public async Task DropFavorite(long id, string dokklasse)
		{
			try
			{
				Favoriten fav = await WorkingContext.Favoritens
					.Where(f => f.NutzerId == id && f.Dokumentenklasse == dokklasse)
					.FirstAsync();
				WorkingContext.Favoritens.Remove(fav);
				await WorkingContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Löschen des Favoriten: " + ex.Message);
			}
		}
	}
}
