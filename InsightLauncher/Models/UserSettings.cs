namespace InsightLauncher.Models;

public class UserSettings
{
    public bool IsDarkMode { get; set; } = true;
    public bool ShowWeather { get; set; } = true;
    public bool EnableNotifications { get; set; } = true;
    public int ReminderMinutesBefore { get; set; } = 5;
    public string? TeamsWebhookUrl { get; set; }
    public string? SlackWebhookUrl { get; set; }
    public List<string> PinnedFileIds { get; set; } = new();
    public List<QuickAction> QuickActions { get; set; } = new();
}
