using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class AnnouncementsViewModel : ObservableObject
{
    private readonly IAnnouncementService _announcementService;

    [ObservableProperty]
    private ObservableCollection<Announcement> _announcements = new();

    [ObservableProperty]
    private Announcement? _selectedAnnouncement;

    [ObservableProperty]
    private bool _isLoading;

    public AnnouncementsViewModel(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
        LoadAnnouncementsAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadAnnouncementsAsync();
    }

    [RelayCommand]
    private void SelectAnnouncement(Announcement announcement)
    {
        SelectedAnnouncement = announcement;
    }

    [RelayCommand]
    private void ClearSelection()
    {
        SelectedAnnouncement = null;
    }

    [RelayCommand]
    private async Task MarkAsReadAsync(Announcement announcement)
    {
        await _announcementService.MarkAsReadAsync(announcement.Id);
        announcement.IsRead = true;
        announcement.IsNew = false;
    }

    private async Task LoadAnnouncementsAsync()
    {
        IsLoading = true;
        try
        {
            var announcements = await _announcementService.GetAnnouncementsAsync();
            Announcements.Clear();
            foreach (var a in announcements.OrderByDescending(a => a.Date))
            {
                Announcements.Add(a);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}
