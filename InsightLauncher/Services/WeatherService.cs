using System.Net.Http;
using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<Weather?> GetCurrentWeatherAsync()
    {
        // デモ用の天気データを返す
        // 実際の実装では OpenWeatherMap API などを使用
        var weather = new Weather
        {
            Temperature = 12,
            Description = "晴れ",
            Icon = "01d",
            Humidity = 45,
            WindSpeed = 3.2
        };

        return Task.FromResult<Weather?>(weather);
    }
}
