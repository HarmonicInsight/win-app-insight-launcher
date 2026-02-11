using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ISearchService _searchService;
    private readonly ISettingsService _settingsService;
    private readonly IRecentFilesService _recentFilesService;
    private readonly IAnnouncementService _announcementService;
    private readonly ISurveyService _surveyService;
    private readonly ITodoService _todoService;

    [ObservableProperty]
    private string _title = "Insight Launcher";

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private ObservableCollection<SearchResult> _searchResults = new();

    [ObservableProperty]
    private bool _isSearching;

    [ObservableProperty]
    private bool _showSearchResults;

    [ObservableProperty]
    private int _unreadAnnouncementCount;

    [ObservableProperty]
    private int _pendingSurveyCount;

    [ObservableProperty]
    private int _pendingTodoCount;

    [ObservableProperty]
    private int _upcomingEventCount;

    [ObservableProperty]
    private bool _isDarkMode = true;

    public MainViewModel(
        ISearchService searchService,
        ISettingsService settingsService,
        IRecentFilesService recentFilesService,
        IAnnouncementService announcementService,
        ISurveyService surveyService,
        ITodoService todoService)
    {
        _searchService = searchService;
        _settingsService = settingsService;
        _recentFilesService = recentFilesService;
        _announcementService = announcementService;
        _surveyService = surveyService;
        _todoService = todoService;

        IsDarkMode = _settingsService.Settings.IsDarkMode;
        _ = LoadBadgeCountsAsync();
    }

    partial void OnSearchQueryChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            ShowSearchResults = false;
            SearchResults.Clear();
            return;
        }

        _ = SearchAsync(value);
    }

    private async Task SearchAsync(string query)
    {
        IsSearching = true;
        try
        {
            var results = await _searchService.SearchAsync(query);
            SearchResults.Clear();
            foreach (var result in results)
            {
                SearchResults.Add(result);
            }
            ShowSearchResults = SearchResults.Count > 0;
        }
        finally
        {
            IsSearching = false;
        }
    }

    [RelayCommand]
    private void ExecuteSearchResult(SearchResult result)
    {
        switch (result.Type)
        {
            case SearchResultType.File:
                if (result.Data is RecentFile file)
                {
                    _recentFilesService.OpenFile(file.Path);
                }
                break;

            case SearchResultType.QuickAction:
                if (result.Data is QuickAction action)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = action.Path,
                        UseShellExecute = true
                    });
                }
                break;

            case SearchResultType.Event:
            case SearchResultType.Announcement:
            case SearchResultType.Task:
                // タブを切り替える（実装は後で）
                break;
        }

        SearchQuery = string.Empty;
        ShowSearchResults = false;
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchQuery = string.Empty;
        ShowSearchResults = false;
        SearchResults.Clear();
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        _settingsService.ToggleDarkMode();
        IsDarkMode = _settingsService.Settings.IsDarkMode;
        ThemeChanged?.Invoke(this, IsDarkMode);
    }

    private async Task LoadBadgeCountsAsync()
    {
        var announcements = await _announcementService.GetAnnouncementsAsync();
        UnreadAnnouncementCount = announcements.Count(a => a.IsNew);

        var surveys = await _surveyService.GetSurveysAsync();
        PendingSurveyCount = surveys.Count(s => !s.IsCompleted);

        PendingTodoCount = _todoService.GetPendingCount();
    }

    [RelayCommand]
    private void Minimize()
    {
        if (Application.Current.MainWindow != null)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }

    [RelayCommand]
    private void Maximize()
    {
        if (Application.Current.MainWindow != null)
        {
            Application.Current.MainWindow.WindowState =
                Application.Current.MainWindow.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
        }
    }

    [RelayCommand]
    private void Close()
    {
        Application.Current.MainWindow?.Close();
    }

    public event EventHandler<bool>? ThemeChanged;
}
