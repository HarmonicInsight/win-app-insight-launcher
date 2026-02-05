namespace InsightLauncher.Models;

public class Announcement
{
    public string Id { get; set; } = string.Empty;
    public AnnouncementType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsNew { get; set; }
    public bool IsRead { get; set; }
}

public enum AnnouncementType
{
    Important,
    Info,
    Schedule
}
