using CryptoDLL;
using CurrieTechnologies.Razor.SweetAlert2;
using DataAccessDLL.Interfaces;
using DataAccessDLL.Modell;
using DataAccessDLL.Services;
using FaxMailFrontend.Data;
using FaxMailFrontend.ViewModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NLog.Web;
using static Org.BouncyCastle.Math.EC.ECCurve;


namespace FaxMailFrontend
{
	public static class RegisterServices
	{
		public static void ConfigureServices(this WebApplicationBuilder builder)
		{
			try
			{
				builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("EntraID"));
				builder.Services.AddControllersWithViews()
					.AddMicrosoftIdentityUI();

				builder.Services.AddAuthorization(options =>
				{
					options.FallbackPolicy = options.DefaultPolicy;
				});

				builder.Services.AddRazorPages();
				builder.Services.AddServerSideBlazor()
					.AddMicrosoftIdentityConsentHandler();
				//Configure Logging
				builder.Services.AddLogging(loggingBuilder =>
				{
					loggingBuilder.ClearProviders();
					loggingBuilder.AddNLogWeb();
				});
				builder.Services.AddSweetAlert2();

				builder.Services.AddScoped<IDokuService, DokuService>();
				builder.Services.AddScoped<IStammDatenService, StammDatenService>();
				builder.Services.AddScoped<IFavoritenService, FavoritenService>();
				builder.Services.AddScoped<IReportingService, ReportingService>();

				builder.Services.AddScoped<FileHandler>(provider =>
				{
					return new FileHandler(
						provider.GetRequiredService<IDokuService>(),
						provider.GetRequiredService<IStammDatenService>(),
						new Nutzer()
					);
				});
				builder.Services.AddScoped<ErrorHandler>(provider => new ErrorHandler(ErrorCode.KeinFehler, ""));
				builder.Services.AddScoped<IWorkingContext>(provider => new WorkingContext(GetWorkingContext(builder.Configuration, provider.GetRequiredService<ILogger<WorkingContext>>())));
				builder.Services.AddScoped<IStammDatenContext>(provider => new StammDatenContext(GetStammDatenContext(builder.Configuration, provider.GetRequiredService<ILogger<StammDatenContext>>())));
				builder.Services.AddScoped<IReportContext>(provider => new ReportContext(GetReportContext(builder.Configuration, provider.GetRequiredService<ILogger<ReportContext>>())));
				builder.Configuration
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
					.AddEnvironmentVariables();
			}
			catch (Exception ex)
			{
				throw new Exception("Error getting working context", ex);
			}
		}

		private static string GetWorkingContext(IConfiguration config, ILogger logger)
		{
			try
			{
				string connectionstring = config.GetValue<string>("ConfigDBSettings:ConnectionString")!;
				string dbuser = config.GetValue<string>("ConfigDBSettings:User")!;
				string dbpassword = config.GetValue<string>("ConfigDBSettings:Password")!;
				if (dbpassword != "")
					dbpassword = CryptoProvider.DecryptString(dbpassword)!;
				if (!connectionstring.Contains("Integrated Security=True"))
				{
					connectionstring += "User ID=" + dbuser + ";Password=" + dbpassword;
				}
				return connectionstring;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error getting working context");
				throw new Exception("Error getting working context", ex);
			}

		}
		private static string GetStammDatenContext(IConfiguration config, ILogger logger)
		{
			try
			{
				string connectionstring = config.GetValue<string>("MasterDBSettings:ConnectionString")!;
				string dbuser = config.GetValue<string>("MasterDBSettings:User")!;
				string dbpassword = config.GetValue<string>("MasterDBSettings:Password")!;
				if (dbpassword != "")
					dbpassword = CryptoProvider.DecryptString(dbpassword)!;
				if (!connectionstring.Contains("Integrated Security=True"))
				{
					connectionstring += "User ID=" + dbuser + ";Password=" + dbpassword;
				}
				return connectionstring;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error getting stamm context");
				throw new Exception("Error getting stamm context", ex);
			}
		}
		private static string GetReportContext(IConfiguration config, ILogger logger)
		{
			try
			{
				string connectionstring = config.GetValue<string>("ReportingDBSettings:ConnectionString")!;
				string dbuser = config.GetValue<string>("ReportingDBSettings:User")!;
				string dbpassword = config.GetValue<string>("ReportingDBSettings:Password")!;
				if (dbpassword != "")
					dbpassword = CryptoProvider.DecryptString(dbpassword)!;
				if (!connectionstring.Contains("Integrated Security=True"))
				{
					connectionstring += "User ID=" + dbuser + ";Password=" + dbpassword;
				}
				return connectionstring;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error getting stamm context");
				throw new Exception("Error getting stamm context", ex);
			}
		}
	}
}
