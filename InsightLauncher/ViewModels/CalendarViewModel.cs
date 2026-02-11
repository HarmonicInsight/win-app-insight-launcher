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
    private ObservableCollection<CalendarDay> _calendarDays = new();

    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private DateTime _displayMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

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
        BuildCalendarDays();
        _ = LoadEventsAsync();
    }

    public string SelectedDateString => $"{SelectedDate:yyyy年M月d日} ({GetDayName(SelectedDate.DayOfWeek)})";
    public string DisplayMonthString => $"{DisplayMonth:yyyy年M月}";
    public bool IsToday => SelectedDate.Date == DateTime.Today;

    [RelayCommand]
    private void PreviousMonth()
    {
        DisplayMonth = DisplayMonth.AddMonths(-1);
        OnPropertyChanged(nameof(DisplayMonthString));
        BuildCalendarDays();
    }

    [RelayCommand]
    private void NextMonth()
    {
        DisplayMonth = DisplayMonth.AddMonths(1);
        OnPropertyChanged(nameof(DisplayMonthString));
        BuildCalendarDays();
    }

    [RelayCommand]
    private void SelectDate(CalendarDay day)
    {
        if (day == null) return;

        SelectedDate = day.Date;
        OnPropertyChanged(nameof(SelectedDateString));
        OnPropertyChanged(nameof(IsToday));

        // 選択状態を更新
        foreach (var d in CalendarDays)
        {
            d.IsSelected = d.Date == SelectedDate;
        }
        OnPropertyChanged(nameof(CalendarDays));

        _ = LoadEventsAsync();
    }

    [RelayCommand]
    private void GoToToday()
    {
        SelectedDate = DateTime.Today;
        DisplayMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        OnPropertyChanged(nameof(SelectedDateString));
        OnPropertyChanged(nameof(DisplayMonthString));
        OnPropertyChanged(nameof(IsToday));
        BuildCalendarDays();
        _ = LoadEventsAsync();
    }

    private void BuildCalendarDays()
    {
        CalendarDays.Clear();

        var firstDayOfMonth = DisplayMonth;
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        // 週の開始日（日曜日）まで戻る
        var startDate = firstDayOfMonth;
        while (startDate.DayOfWeek != DayOfWeek.Sunday)
        {
            startDate = startDate.AddDays(-1);
        }

        // 6週間分のカレンダーを生成（42日）
        for (int i = 0; i < 42; i++)
        {
            var date = startDate.AddDays(i);
            CalendarDays.Add(new CalendarDay
            {
                Date = date,
                IsCurrentMonth = date.Month == DisplayMonth.Month,
                IsSelected = date.Date == SelectedDate.Date
            });
        }
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
