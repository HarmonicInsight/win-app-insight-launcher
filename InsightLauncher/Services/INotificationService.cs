using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface INotificationService
{
    void ShowNotification(string title, string message);
    void ScheduleEventReminder(CalendarEvent evt, int minutesBefore);
    void CancelReminder(string eventId);
    void StartReminderService();
    void StopReminderService();
}
