using DataAccessDLL.Modell;
using Microsoft.EntityFrameworkCore;

namespace DataAccessDLL.Interfaces
{
	public interface IReportContext
	{
		DbSet<Frontendreport> Frontendreports { get; set; }
	}
}