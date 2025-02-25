using Microsoft.EntityFrameworkCore;
using DataAccessDLL.Modell;

namespace DataAccessDLL.Interfaces

{
	public interface IStammDatenContext
	{
		DbSet<LeistungserbringerIk> LeistungserbringerIks { get; set; }
		DbSet<StammdatenFirmenkunde> StammdatenFirmenkundes { get; set; }
		DbSet<StammdatenVersicherte> StammdatenVersichertes { get; set; }
	}
}