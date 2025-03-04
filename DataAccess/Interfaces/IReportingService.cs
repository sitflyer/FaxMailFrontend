using DataAccessDLL.Modell;

namespace DataAccessDLL.Interfaces
{
	public interface IReportingService
	{
		Task AddReport(Frontendreport report);
	}
}