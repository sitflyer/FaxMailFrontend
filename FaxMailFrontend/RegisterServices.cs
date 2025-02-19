using FaxMailFrontend.Data;
using FaxMailFrontend.ViewModel;
using Microsoft.Identity.Web;

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
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<FileHandler>(provider => new FileHandler("", ""));
			builder.Services.AddScoped<ErrorHandler>(provider => new ErrorHandler(ErrorCode.KeinFehler, ""));
			//builder.Services.AddSweetAlert2();
		}
	}
}
