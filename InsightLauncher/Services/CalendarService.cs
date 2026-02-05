using System.Diagnostics;
using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class CalendarService : ICalendarService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public bool IsOutlookConnected { get; private set; } = true; // デモ用
    public bool IsGoogleConnected { get; private set; } = true;  // デモ用

    public CalendarService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<List<CalendarEvent>> GetEventsAsync(DateTime date)
    {
        // TODO: 実際の実装では Microsoft Graph API / Google Calendar API から取得

        var today = DateTime.Today;
        var events = new List<CalendarEvent>
        {
            new()
            {
                Id = "1",
                Title = "週次チームミーティング",
                StartTime = today.AddHours(9),
                EndTime = today.AddHours(10),
                Source = CalendarSource.Outlook,
                Location = "会議室A"
            },
            new()
            {
                Id = "2",
                Title = "プロジェクト進捗報告",
                StartTime = today.AddHours(11),
                EndTime = today.AddHours(12),
                Source = CalendarSource.Outlook,
                Location = "Teams"
            },
            new()
            {
                Id = "3",
                Title = "昼食会（新人歓迎）",
                StartTime = today.AddHours(12).AddMinutes(30),
                EndTime = today.AddHours(13).AddMinutes(30),
                Source = CalendarSource.Google,
                Location = "社員食堂"
            },
            new()
            {
                Id = "4",
                Title = "クライアント打ち合わせ",
                StartTime = today.AddHours(14),
                EndTime = today.AddHours(15).AddMinutes(30),
                Source = CalendarSource.Outlook,
                Location = "外出先"
            },
            new()
            {
                Id = "5",
                Title = "歯医者の予約",
                StartTime = today.AddHours(18),
                EndTime = today.AddHours(19),
                Source = CalendarSource.Google,
                Location = "〇〇歯科"
            }
        };

        return Task.FromResult(events);
    }

    public Task<bool> ConnectOutlookAsync()
    {
        // TODO: Microsoft Graph API OAuth フロー
        // https://login.microsoftonline.com/common/oauth2/v2.0/authorize
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://outlook.office.com/calendar",
                UseShellExecute = true
            });
            IsOutlookConnected = true;
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> ConnectGoogleAsync()
    {
        // TODO: Google Calendar API OAuth フロー
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://calendar.google.com",
                UseShellExecute = true
            });
            IsGoogleConnected = true;
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
