using DataAccessDLL.Interfaces;
using DataAccessDLL.Modell;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDLL.Services
{

	public class FavoritenService : IFavoritenService
	{
		private IWorkingContext WorkingContext;
		public FavoritenService(IWorkingContext mycontext)
		{
			WorkingContext = mycontext;
		}
		public async Task<List<INutzer>> GetAllNutzer()
		{
			try
			{
				return await WorkingContext.Nutzers.Cast<INutzer>().ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Nutzers: " + ex.Message);
			}
		}

		public async Task<List<IFavoriten>> GetFavoritenByNutzer(string NutzerKennung)
		{
			try
			{
				return await WorkingContext.Favoritens
					.Where(f => f.Nutzer.Kennung == NutzerKennung)
					.Cast<IFavoriten>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Favoriten: " + ex.Message);
			}
		}
		public async Task<bool> IsNutzerExisting(string NutzerKennung)
		{
			try
			{
				return await WorkingContext.Nutzers
					.Where(n => n.Kennung == NutzerKennung)
					.AnyAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Nutzers: " + ex.Message);
			}
		}
		public async Task<bool> IsFavoritExistingByUser(string NutzerKennung, string Dokumentenklasse)
		{
			try
			{
				return await WorkingContext.Favoritens
					.Where(f => f.Nutzer.Kennung == NutzerKennung && f.Dokumentenklasse == Dokumentenklasse)
					.AnyAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Favoriten: " + ex.Message);
			}
		}

		public async Task<long> GetUserIDFromKennung(string NutzerKennung)
		{
			try
			{
				return await WorkingContext.Nutzers
					.Where(n => n.Kennung == NutzerKennung)
					.Select(n => n.Id)
					.FirstAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Nutzers: " + ex.Message);
			}
		}

		public async Task<long> CreateNutzerByKennungAndReturnNewID(INutzer nutzer)
		{
			try
			{
				var newNutzer = new Nutzer
				{
					Vorname = nutzer.Vorname,
					Nachname = nutzer.Nachname,
					Telefonnummer = nutzer.Telefonnummer,
					Email = nutzer.Email,
					Kennung = nutzer.Kennung
				};

				await WorkingContext.Nutzers.AddAsync(newNutzer);
				await WorkingContext.SaveChangesAsync();

				return newNutzer.Id;
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Erstellen des Nutzers: " + ex.Message);
			}
		}
	}
}
