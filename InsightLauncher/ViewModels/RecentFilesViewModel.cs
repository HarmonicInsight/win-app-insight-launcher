using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class RecentFilesViewModel : ObservableObject
{
    private readonly IRecentFilesService _recentFilesService;

    [ObservableProperty]
    private ObservableCollection<RecentFile> _files = new();

    [ObservableProperty]
    private ObservableCollection<RecentFile> _filteredFiles = new();

    [ObservableProperty]
    private FileType? _selectedFilter;

    [ObservableProperty]
    private bool _isLoading;

    public RecentFilesViewModel(IRecentFilesService recentFilesService)
    {
        _recentFilesService = recentFilesService;
        LoadFilesAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadFilesAsync();
    }

    [RelayCommand]
    private void SetFilter(FileType? filter)
    {
        SelectedFilter = filter;
        ApplyFilter();
    }

    [RelayCommand]
    private void OpenFile(RecentFile file)
    {
        _recentFilesService.OpenFile(file.Path);
    }

    [RelayCommand]
    private void OpenExplorer()
    {
        _recentFilesService.OpenExplorer();
    }

    private async Task LoadFilesAsync()
    {
        IsLoading = true;
        try
        {
            var files = await _recentFilesService.GetRecentFilesAsync();
            Files.Clear();
            foreach (var file in files)
            {
                Files.Add(file);
            }
            ApplyFilter();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ApplyFilter()
    {
        FilteredFiles.Clear();
        var filtered = SelectedFilter.HasValue
            ? Files.Where(f => f.Type == SelectedFilter.Value)
            : Files;

        foreach (var file in filtered)
        {
            FilteredFiles.Add(file);
        }
    }

    public string GetRelativeTime(DateTime lastModified)
    {
        var diff = DateTime.Now - lastModified;

        if (diff.TotalMinutes < 60)
            return "数分前";
        if (diff.TotalHours < 24)
            return $"{(int)diff.TotalHours}時間前";
        if (diff.TotalDays < 7)
            return $"{(int)diff.TotalDays}日前";

        return lastModified.ToString("M月d日");
    }
}
