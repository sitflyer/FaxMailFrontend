using DataAccessDLL.Modell;
using Microsoft.EntityFrameworkCore;

namespace DataAccessDLL.Interfaces
{
	public interface IWorkingContext
	{
		DbSet<DokumentenProcessor> DokumentenProcessors { get; set; }
	}
}