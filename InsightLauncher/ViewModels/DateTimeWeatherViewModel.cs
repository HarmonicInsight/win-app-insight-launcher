using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class DateTimeWeatherViewModel : ObservableObject
{
    private readonly IWeatherService _weatherService;
    private readonly DispatcherTimer _timer;
    private static readonly string[] DayNames = { "日", "月", "火", "水", "木", "金", "土" };

    [ObservableProperty]
    private string _currentDate = string.Empty;

    [ObservableProperty]
    private string _currentTime = string.Empty;

    [ObservableProperty]
    private Weather? _weather;

    public DateTimeWeatherViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;

        UpdateDateTime();

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += (_, _) => UpdateDateTime();
        _timer.Start();

        LoadWeatherAsync();
    }

    private void UpdateDateTime()
    {
        var now = DateTime.Now;
        CurrentDate = $"{now:yyyy年M月d日} ({DayNames[(int)now.DayOfWeek]})";
        CurrentTime = now.ToString("HH:mm:ss");
    }

    private async void LoadWeatherAsync()
    {
        Weather = await _weatherService.GetCurrentWeatherAsync();
    }
}
