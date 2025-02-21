using DataAccessDLL.Modell;
using Microsoft.EntityFrameworkCore;

namespace DataAccessDLL.Interfaces
{
	public interface IStammDatenContext
	{
		DbSet<StammdatenVersicherte> StammdatenVersichertes { get; set; }
	}
}