using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class SearchService : ISearchService
{
    private readonly IRecentFilesService _recentFilesService;
    private readonly ICalendarService _calendarService;
    private readonly IAnnouncementService _announcementService;
    private readonly ISettingsService _settingsService;
    private readonly ITodoService _todoService;

    public SearchService(
        IRecentFilesService recentFilesService,
        ICalendarService calendarService,
        IAnnouncementService announcementService,
        ISettingsService settingsService,
        ITodoService todoService)
    {
        _recentFilesService = recentFilesService;
        _calendarService = calendarService;
        _announcementService = announcementService;
        _settingsService = settingsService;
        _todoService = todoService;
    }

    public async Task<IEnumerable<SearchResult>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return Enumerable.Empty<SearchResult>();

        var results = new List<SearchResult>();
        var lowerQuery = query.ToLowerInvariant();

        // ファイル検索
        var files = await _recentFilesService.GetRecentFilesAsync();
        foreach (var file in files.Where(f => f.Name.ToLowerInvariant().Contains(lowerQuery)))
        {
            results.Add(new SearchResult
            {
                Id = file.Path,
                Title = file.Name,
                Subtitle = file.FolderPath,
                Icon = GetFileIcon(file.Type),
                Type = SearchResultType.File,
                Data = file
            });
        }

        // イベント検索
        var events = await _calendarService.GetEventsAsync(DateTime.Today);
        var futureEvents = await _calendarService.GetEventsAsync(DateTime.Today.AddDays(7));
        var allEvents = events.Concat(futureEvents).DistinctBy(e => e.Id);

        foreach (var evt in allEvents.Where(e => e.Title.ToLowerInvariant().Contains(lowerQuery)))
        {
            results.Add(new SearchResult
            {
                Id = evt.Id,
                Title = evt.Title,
                Subtitle = $"{evt.StartTime:M/d HH:mm} - {evt.EndTime:HH:mm}",
                Icon = "📅",
                Type = SearchResultType.Event,
                Data = evt
            });
        }

        // お知らせ検索
        var announcements = await _announcementService.GetAnnouncementsAsync();
        foreach (var ann in announcements.Where(a =>
            a.Title.ToLowerInvariant().Contains(lowerQuery) ||
            a.Content.ToLowerInvariant().Contains(lowerQuery)))
        {
            results.Add(new SearchResult
            {
                Id = ann.Id,
                Title = ann.Title,
                Subtitle = ann.Content.Length > 50 ? ann.Content[..50] + "..." : ann.Content,
                Icon = "🔔",
                Type = SearchResultType.Announcement,
                Data = ann
            });
        }

        // クイックアクション検索
        foreach (var action in _settingsService.Settings.QuickActions
            .Where(a => a.Name.ToLowerInvariant().Contains(lowerQuery)))
        {
            results.Add(new SearchResult
            {
                Id = action.Id,
                Title = action.Name,
                Subtitle = action.Path,
                Icon = action.Icon,
                Type = SearchResultType.QuickAction,
                Data = action
            });
        }

        // タスク検索
        var todos = await _todoService.GetTodosAsync();
        foreach (var todo in todos.Where(t => t.Title.ToLowerInvariant().Contains(lowerQuery)))
        {
            results.Add(new SearchResult
            {
                Id = todo.Id,
                Title = todo.Title,
                Subtitle = todo.DueDate?.ToString("M/d 期限") ?? "期限なし",
                Icon = todo.IsCompleted ? "✅" : "☐",
                Type = SearchResultType.Task,
                Data = todo
            });
        }

        return results.Take(20);
    }

    private static string GetFileIcon(FileType type) => type switch
    {
        FileType.Excel => "📊",
        FileType.Word => "📝",
        FileType.PowerPoint => "📽️",
        _ => "📄"
    };
}
