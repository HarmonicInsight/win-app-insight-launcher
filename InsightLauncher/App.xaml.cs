using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using InsightLauncher.Services;
using InsightLauncher.ViewModels;
using InsightLauncher.Views;

namespace InsightLauncher;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            // 通知サービス開始
            var notificationService = Services.GetRequiredService<INotificationService>();
            notificationService.StartReminderService();

            // メインウィンドウを表示
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"起動エラー:\n{ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        var notificationService = Services?.GetService<INotificationService>();
        notificationService?.StopReminderService();
        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Services
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IRecentFilesService, RecentFilesService>();
        services.AddSingleton<IWeatherService, WeatherService>();
        services.AddSingleton<IAnnouncementService, AnnouncementService>();
        services.AddSingleton<ISurveyService, SurveyService>();
        services.AddSingleton<ICalendarService, CalendarService>();
        services.AddSingleton<ITodoService, TodoService>();
        services.AddSingleton<ITeamService, TeamService>();
        services.AddSingleton<ISearchService, SearchService>();
        services.AddSingleton<INotificationService, NotificationService>();

        // HttpClient
        services.AddHttpClient();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<DateTimeWeatherViewModel>();
        services.AddTransient<CalendarViewModel>();
        services.AddTransient<AnnouncementsViewModel>();
        services.AddTransient<RecentFilesViewModel>();
        services.AddTransient<SurveyViewModel>();
        services.AddTransient<TodoViewModel>();
        services.AddTransient<QuickActionsViewModel>();
        services.AddTransient<TeamViewModel>();
        services.AddTransient<SettingsViewModel>();
    }
}
