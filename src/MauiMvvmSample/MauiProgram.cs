using System.Reflection;
using CommunityToolkit.Maui;
using MauiMvvmSample.Options;
using MauiMvvmSample.Services;
using MauiMvvmSample.ViewModels;
using MauiMvvmSample.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace MauiMvvmSample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{nameof(MauiMvvmSample)}.appsettings.json");
        var configurationBuilder = new ConfigurationBuilder().AddJsonStream(stream!);
#if DEV
        using var additionalStream = assembly.GetManifestResourceStream($"{nameof(MauiMvvmSample)}.appsettings.Development.json");

        configurationBuilder.AddJsonStream(additionalStream!);
#elif STG
        using var additionalStream = assembly.GetManifestResourceStream($"{nameof(MauiMvvmSample)}.appsettings.Stage.json");

        configurationBuilder.AddJsonStream(additionalStream!);
#elif PROD
        using var additionalStream = assembly.GetManifestResourceStream($"{nameof(MauiMvvmSample)}.appsettings.Production.json");

        configurationBuilder.AddJsonStream(additionalStream!);
#endif

        builder.Configuration.AddConfiguration(configurationBuilder.Build());

#if DEBUG
		builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<BackgroundService>();
        builder.Services.AddTransient<MainPage, MainPageViewModel>();

		return builder.Build();
	}
}
