using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessDLL.Modell;
using DataAccessDLL.Interfaces;
using Microsoft.EntityFrameworkCore;

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
	}
}
