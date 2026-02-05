using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface ICalendarService
{
    Task<List<CalendarEvent>> GetEventsAsync(DateTime date);
    Task<bool> ConnectOutlookAsync();
    Task<bool> ConnectGoogleAsync();
    bool IsOutlookConnected { get; }
    bool IsGoogleConnected { get; }
}
