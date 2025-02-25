using CurrieTechnologies.Razor.SweetAlert2;
using FaxMailFrontend.Data;
using FaxMailFrontend.ViewModel;
using Microsoft.Identity.Web;
using NLog.Web;
using static MsgReader.Outlook.Storage;
using DataAccessDLL.Interfaces;
using DataAccessDLL.Modell;
using DataAccessDLL.Services;


namespace FaxMailFrontend
{
	public static class RegisterServices
	{
		public static void ConfigureServices(this WebApplicationBuilder builder)
		{
			// Add services to the container.
			//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
			//builder.Services.AddControllersWithViews()
			//    .AddMicrosoftIdentityUI();

			//builder.Services.AddAuthorization(options =>
			//{
			//    // By default, all incoming requests will be authorized according to the default policy
			//    options.FallbackPolicy = options.DefaultPolicy;
			//});
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
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IDokuService, DokuService>();
			builder.Services.AddScoped<IStammDatenService,StammDatenService>();
			builder.Services.AddScoped<FileHandler>();
			builder.Services.AddScoped<ErrorHandler>(provider => new ErrorHandler(ErrorCode.KeinFehler, ""));
			builder.Services.AddScoped<IWorkingContext>(provider => new WorkingContext(GetWorkingCntext()));
			builder.Services.AddScoped<IStammDatenContext>(provider => new StammDatenContext(GetStammDatenContext()));
			//builder.Services.AddScoped<IReportingContext>(provider => new ReportingContext("Data Source=localhost;Initial Catalog=StammDaten;Integrated Security=True;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
			builder.Configuration
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			//builder.Services.AddSweetAlert2();
		}

		private static string GetWorkingCntext()
		{
			return "Data Source=localhost;Initial Catalog=Nifi;Integrated Security=True;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		}
		private static string GetStammDatenContext()
		{
			return "Data Source=localhost;Initial Catalog=StammDaten;Integrated Security=True;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		}
	}
}
