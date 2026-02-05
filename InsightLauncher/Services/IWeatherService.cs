using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface IWeatherService
{
    Task<Weather?> GetCurrentWeatherAsync();
}
