using DataAccessDLL.Modell;
using Microsoft.EntityFrameworkCore;

namespace DataAccessDLL.Interfaces
{
	public interface IStammDatenContext
	{
		DbSet<GposListe> GposListes { get; set; }
		DbSet<LeistungserbringerLanr> LeistungserbringerLanrs { get; set; }
		DbSet<StammdatenFirmenkunde> StammdatenFirmenkundes { get; set; }
		DbSet<StammdatenVersicherte> StammdatenVersichertes { get; set; }
	}
}