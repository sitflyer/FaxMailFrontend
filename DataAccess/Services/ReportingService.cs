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
	public class ReportingService : IReportingService
	{
		private readonly IReportContext ReportingContext;

		public ReportingService(IReportContext reportingContext)
		{
			ReportingContext = reportingContext;
		}

		public async Task AddReport(Frontendreport report)
		{
			try
			{
				ReportingContext.Frontendreports.Add(report);
				await ((DbContext)ReportingContext).SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Fehler beim Schreiben des Reports: " + ex.Message);
			}
		}
	}
}