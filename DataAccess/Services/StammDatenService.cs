using DataAccessDLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDLL.Services
{
	public class StammDatenService(IStammDatenContext mycontext) : IStammDatenService
	{
		private readonly IStammDatenContext StammContext = mycontext;

		public async Task<List<IStammdatenVersicherte>> GetAllVersicherte()
		{
			try
			{
				return await StammContext.StammdatenVersichertes.Cast<IStammdatenVersicherte>().ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}
		public async Task<List<IStammdatenVersicherte>> GetVersichertenByKVNR9(string kvnr9)
		{
			try
			{
				return await StammContext.StammdatenVersichertes
					.Where(sv => sv.Kvnr9 == kvnr9)
					.Cast<IStammdatenVersicherte>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}
		public async Task<List<IStammdatenVersicherte>> GetVersichertenByKVNR10(string kvnr10)
		{
			try
			{
				return await StammContext.StammdatenVersichertes
					.Where(sv => sv.Kvnr10 == kvnr10)
					.Cast<IStammdatenVersicherte>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}
		public async Task<List<IStammdatenVersicherte>> GetVersichertenByBPNR(string bprn)
		{
			try
			{
				return await StammContext.StammdatenVersichertes
					.Where(sv => sv.Bpnr == bprn)
					.Cast<IStammdatenVersicherte>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}
		public async Task<List<IStammdatenVersicherte>> GetVersichertenByRVNR(string rvnr)
		{
			try
			{
				return await StammContext.StammdatenVersichertes
					.Where(sv => sv.Rvnr == rvnr)
					.Cast<IStammdatenVersicherte>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}


	}
}
