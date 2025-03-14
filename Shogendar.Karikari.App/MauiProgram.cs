using System.Globalization;
using Microsoft.Extensions.Logging;
using Radzen;

namespace Shogendar.Karikari.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddRadzenComponents();
		builder.Services.AddMauiBlazorWebView();

		APIClient.Instance = new APIClient("https://karikari-api.wsnet.jp/api", "token", "secret");

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
