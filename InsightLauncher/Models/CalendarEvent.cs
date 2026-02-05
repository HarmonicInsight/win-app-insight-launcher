namespace InsightLauncher.Models;

public class CalendarEvent
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public CalendarSource Source { get; set; }
    public string? Location { get; set; }
    public bool IsAllDay { get; set; }
}

public enum CalendarSource
{
    Outlook,
    Google
}
