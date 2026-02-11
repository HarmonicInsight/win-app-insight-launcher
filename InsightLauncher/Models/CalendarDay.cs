namespace InsightLauncher.Models;

public class CalendarDay
{
    public DateTime Date { get; set; }
    public int Day => Date.Day;
    public bool IsCurrentMonth { get; set; }
    public bool IsToday => Date.Date == DateTime.Today;
    public bool IsSelected { get; set; }
    public bool HasEvents { get; set; }
    public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;
}
