using DataAccessDLL.Interfaces;
using Microsoft.EntityFrameworkCore;

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

		public List<IStammdatenVersicherte> GetAllVersicherteSync()
		{
			try
			{
				return StammContext.StammdatenVersichertes.Cast<IStammdatenVersicherte>().ToList();
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

		public List<IStammdatenVersicherte> GetVersichertenByKVNR9Sync(string kvnr9)
		{
			try
			{
				return StammContext.StammdatenVersichertes
					.Where(sv => sv.Kvnr9 == kvnr9)
					.Cast<IStammdatenVersicherte>()
					.ToList();
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
		public List<IStammdatenVersicherte> GetVersichertenByKVNR10Sync(string kvnr10)
		{
			try
			{
				return StammContext.StammdatenVersichertes
					.Where(sv => sv.Kvnr10 == kvnr10)
					.Cast<IStammdatenVersicherte>()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}
		public async Task<List<IStammdatenVersicherte>> GetVersichertenByBPNR(string bpnr)
		{
			try
			{
				return await StammContext.StammdatenVersichertes
					.Where(sv => sv.Bpnr == bpnr)
					.Cast<IStammdatenVersicherte>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}
		public List<IStammdatenVersicherte> GetVersichertenByBPNRSync(string bpnr)
		{
			try
			{
				return StammContext.StammdatenVersichertes
					.Where(sv => sv.Bpnr == bpnr)
					.Cast<IStammdatenVersicherte>()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Versicherten: " + ex.Message);
			}
		}
		public async Task<List<IStammdatenFirmenkunde>> GetVersichertenByBTNR(string btnr)
		{
			try
			{
				return await StammContext.StammdatenFirmenkundes
					.Where(sv => sv.Btnr == btnr)
					.Cast<IStammdatenFirmenkunde>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Firmenkunden: " + ex.Message);
			}
		}
		public List<IStammdatenFirmenkunde> GetVersichertenByBTNRSync(string btnr)
		{
			try
			{
				return StammContext.StammdatenFirmenkundes
					.Where(sv => sv.Btnr == btnr)
					.Cast<IStammdatenFirmenkunde>()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Firmenkunden: " + ex.Message);
			}
		}
		public async Task<List<ILeistungserbringerLanr>> GetVersichertenByLeik(string bpnr)
		{
			try
			{
				return await StammContext.LeistungserbringerLanrs
					.Where(sv => sv.Bpnr == bpnr)
					.Cast<ILeistungserbringerLanr>()
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Leistungserbringer: " + ex.Message);
			}
		}
		public List<ILeistungserbringerLanr> GetVersichertenByLeikSync(string bpnr)
		{
			try
			{
				return StammContext.LeistungserbringerLanrs
					.Where(sv => sv.Bpnr == bpnr)
					.Cast<ILeistungserbringerLanr>()
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der Leistungserbringer: " + ex.Message);
			}
		}
		public async Task<List<IGposListe>> GetGPosliste()
		{
			try
			{
				return await StammContext.GposListes.Cast<IGposListe>().ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der GPosListe: " + ex.Message);
			}
		}
		public List<IGposListe> GetGPoslisteSync()
		{
			try
			{
				return StammContext.GposListes.Cast<IGposListe>().ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der GPosListe: " + ex.Message);
			}
		}
		public List<string> GetProduktgruppenDistinctSync()
		{
			try
			{
				return StammContext.GposListes
					.Select(g => g.Produktgruppe)
					.Distinct().OrderBy(g => g)
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der GPosListe: " + ex.Message);
			}
		}
		public async Task<List<string>> GetProduktgruppenDistinct()
		{
			try
			{
				return await StammContext.GposListes
					.Select(g => g.Produktgruppe)
					.Distinct().OrderBy(g => g)
					.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Lesen der GPosListe: " + ex.Message);
			}
		}
	}
}
