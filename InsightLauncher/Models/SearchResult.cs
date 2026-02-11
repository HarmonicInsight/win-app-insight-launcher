namespace InsightLauncher.Models;

public enum SearchResultType
{
    File,
    Event,
    Announcement,
    QuickAction,
    Task
}

public class SearchResult
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public SearchResultType Type { get; set; }
    public object? Data { get; set; }
}
