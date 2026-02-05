using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.Services;
using InsightLauncher.ViewModels;

namespace InsightLauncher;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Services
        services.AddSingleton<IRecentFilesService, RecentFilesService>();
        services.AddSingleton<IWeatherService, WeatherService>();
        services.AddSingleton<IAnnouncementService, AnnouncementService>();
        services.AddSingleton<ISurveyService, SurveyService>();
        services.AddSingleton<ICalendarService, CalendarService>();

        // HttpClient
        services.AddHttpClient();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<DateTimeWeatherViewModel>();
        services.AddTransient<CalendarViewModel>();
        services.AddTransient<AnnouncementsViewModel>();
        services.AddTransient<RecentFilesViewModel>();
        services.AddTransient<SurveyViewModel>();
    }
}
