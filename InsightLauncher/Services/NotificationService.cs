using System.Windows;
using System.Windows.Threading;
using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class NotificationService : INotificationService
{
    private readonly ISettingsService _settingsService;
    private readonly ICalendarService _calendarService;
    private readonly Dictionary<string, DispatcherTimer> _reminderTimers = new();
    private DispatcherTimer? _checkTimer;

    public NotificationService(ISettingsService settingsService, ICalendarService calendarService)
    {
        _settingsService = settingsService;
        _calendarService = calendarService;
    }

    public void ShowNotification(string title, string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var notification = new NotificationWindow(title, message);
            notification.Show();
        });
    }

    public void ScheduleEventReminder(CalendarEvent evt, int minutesBefore)
    {
        var reminderTime = evt.StartTime.AddMinutes(-minutesBefore);
        var delay = reminderTime - DateTime.Now;

        if (delay <= TimeSpan.Zero)
            return;

        CancelReminder(evt.Id);

        var timer = new DispatcherTimer
        {
            Interval = delay
        };
        timer.Tick += (s, e) =>
        {
            timer.Stop();
            _reminderTimers.Remove(evt.Id);
            ShowNotification($"予定のリマインダー", $"{evt.Title}\n{evt.StartTime:HH:mm} 開始");
        };
        timer.Start();
        _reminderTimers[evt.Id] = timer;
    }

    public void CancelReminder(string eventId)
    {
        if (_reminderTimers.TryGetValue(eventId, out var timer))
        {
            timer.Stop();
            _reminderTimers.Remove(eventId);
        }
    }

    public void StartReminderService()
    {
        _checkTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(1)
        };
        _checkTimer.Tick += async (s, e) => await CheckUpcomingEventsAsync();
        _checkTimer.Start();

        // 初回チェック
        _ = CheckUpcomingEventsAsync();
    }

    public void StopReminderService()
    {
        _checkTimer?.Stop();
        foreach (var timer in _reminderTimers.Values)
        {
            timer.Stop();
        }
        _reminderTimers.Clear();
    }

    private async Task CheckUpcomingEventsAsync()
    {
        if (!_settingsService.Settings.EnableNotifications)
            return;

        var events = await _calendarService.GetEventsAsync(DateTime.Today);
        var minutesBefore = _settingsService.Settings.ReminderMinutesBefore;

        foreach (var evt in events)
        {
            if (!_reminderTimers.ContainsKey(evt.Id))
            {
                ScheduleEventReminder(evt, minutesBefore);
            }
        }
    }
}

public partial class NotificationWindow : Window
{
    private readonly DispatcherTimer _closeTimer;

    public NotificationWindow(string title, string message)
    {
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
        Background = System.Windows.Media.Brushes.Transparent;
        Topmost = true;
        ShowInTaskbar = false;

        var workArea = SystemParameters.WorkArea;
        Left = workArea.Right - 320;
        Top = workArea.Bottom - 120;
        Width = 300;
        Height = 100;

        var border = new System.Windows.Controls.Border
        {
            Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30)),
            BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(60, 60, 60)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(16)
        };

        var stack = new System.Windows.Controls.StackPanel();

        var titleBlock = new System.Windows.Controls.TextBlock
        {
            Text = title,
            Foreground = System.Windows.Media.Brushes.White,
            FontWeight = FontWeights.SemiBold,
            FontSize = 14
        };

        var messageBlock = new System.Windows.Controls.TextBlock
        {
            Text = message,
            Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(180, 180, 180)),
            FontSize = 12,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 8, 0, 0)
        };

        stack.Children.Add(titleBlock);
        stack.Children.Add(messageBlock);
        border.Child = stack;
        Content = border;

        _closeTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        _closeTimer.Tick += (s, e) =>
        {
            _closeTimer.Stop();
            Close();
        };
        _closeTimer.Start();

        MouseLeftButtonDown += (s, e) => Close();
    }
}
