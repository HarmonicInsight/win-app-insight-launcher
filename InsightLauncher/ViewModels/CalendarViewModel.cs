using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class CalendarViewModel : ObservableObject
{
    private readonly ICalendarService _calendarService;

    [ObservableProperty]
    private ObservableCollection<CalendarEvent> _events = new();

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isOutlookConnected;

    [ObservableProperty]
    private bool _isGoogleConnected;

    public CalendarViewModel(ICalendarService calendarService)
    {
        _calendarService = calendarService;
        IsOutlookConnected = calendarService.IsOutlookConnected;
        IsGoogleConnected = calendarService.IsGoogleConnected;
        LoadEventsAsync();
    }

    public string SelectedDateString => $"{SelectedDate:yyyy年M月d日} ({GetDayName(SelectedDate.DayOfWeek)})";
    public bool IsToday => SelectedDate.Date == DateTime.Today;

    [RelayCommand]
    private void PreviousDay()
    {
        SelectedDate = SelectedDate.AddDays(-1);
        OnPropertyChanged(nameof(SelectedDateString));
        OnPropertyChanged(nameof(IsToday));
        LoadEventsAsync();
    }

    [RelayCommand]
    private void NextDay()
    {
        SelectedDate = SelectedDate.AddDays(1);
        OnPropertyChanged(nameof(SelectedDateString));
        OnPropertyChanged(nameof(IsToday));
        LoadEventsAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadEventsAsync();
    }

    [RelayCommand]
    private async Task ConnectOutlookAsync()
    {
        await _calendarService.ConnectOutlookAsync();
        IsOutlookConnected = _calendarService.IsOutlookConnected;
    }

    [RelayCommand]
    private async Task ConnectGoogleAsync()
    {
        await _calendarService.ConnectGoogleAsync();
        IsGoogleConnected = _calendarService.IsGoogleConnected;
    }

    [RelayCommand]
    private void OpenOutlookCalendar()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://outlook.office.com/calendar",
            UseShellExecute = true
        });
    }

    [RelayCommand]
    private void OpenGoogleCalendar()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://calendar.google.com",
            UseShellExecute = true
        });
    }

    private async Task LoadEventsAsync()
    {
        IsLoading = true;
        try
        {
            var events = await _calendarService.GetEventsAsync(SelectedDate);
            Events.Clear();
            foreach (var e in events.OrderBy(e => e.StartTime))
            {
                Events.Add(e);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static string GetDayName(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Sunday => "日",
            DayOfWeek.Monday => "月",
            DayOfWeek.Tuesday => "火",
            DayOfWeek.Wednesday => "水",
            DayOfWeek.Thursday => "木",
            DayOfWeek.Friday => "金",
            DayOfWeek.Saturday => "土",
            _ => ""
        };
    }
}
