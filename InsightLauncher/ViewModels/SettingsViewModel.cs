using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private bool _isDarkMode;

    [ObservableProperty]
    private bool _showWeather;

    [ObservableProperty]
    private bool _enableNotifications;

    [ObservableProperty]
    private int _reminderMinutesBefore;

    [ObservableProperty]
    private string? _teamsWebhookUrl;

    [ObservableProperty]
    private string? _slackWebhookUrl;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        LoadSettings();
    }

    private void LoadSettings()
    {
        IsDarkMode = _settingsService.Settings.IsDarkMode;
        ShowWeather = _settingsService.Settings.ShowWeather;
        EnableNotifications = _settingsService.Settings.EnableNotifications;
        ReminderMinutesBefore = _settingsService.Settings.ReminderMinutesBefore;
        TeamsWebhookUrl = _settingsService.Settings.TeamsWebhookUrl;
        SlackWebhookUrl = _settingsService.Settings.SlackWebhookUrl;
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        _settingsService.ToggleDarkMode();
        IsDarkMode = _settingsService.Settings.IsDarkMode;
        ThemeChanged?.Invoke(this, IsDarkMode);
    }

    [RelayCommand]
    private void SaveSettings()
    {
        _settingsService.Settings.ShowWeather = ShowWeather;
        _settingsService.Settings.EnableNotifications = EnableNotifications;
        _settingsService.Settings.ReminderMinutesBefore = ReminderMinutesBefore;
        _settingsService.Settings.TeamsWebhookUrl = TeamsWebhookUrl;
        _settingsService.Settings.SlackWebhookUrl = SlackWebhookUrl;
        _settingsService.Save();
    }

    public event EventHandler<bool>? ThemeChanged;

    public static int[] ReminderOptions => new[] { 1, 5, 10, 15, 30 };
}
