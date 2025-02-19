using FaxMailFrontend.Data;
using FaxMailFrontend.ViewModel;
using Microsoft.Identity.Web;
using NLog.Web;
using static MsgReader.Outlook.Storage;

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

			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<FileHandler>(provider => new FileHandler("", ""));
			builder.Services.AddScoped<ErrorHandler>(provider => new ErrorHandler(ErrorCode.KeinFehler, ""));
			builder.Configuration
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			//builder.Services.AddSweetAlert2();
		}
	}
}
