using System.Net.Http;
using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class SurveyService : ISurveyService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SurveyService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<List<Survey>> GetSurveysAsync()
    {
        // TODO: 実際の実装では管理サーバーから取得

        var surveys = new List<Survey>
        {
            new()
            {
                Id = "1",
                Question = "在宅勤務に関するアンケート",
                Type = SurveyType.Single,
                Options = new List<SurveyOption>
                {
                    new() { Id = "a", Label = "週5日出社希望" },
                    new() { Id = "b", Label = "週3日出社希望" },
                    new() { Id = "c", Label = "週1日出社希望" },
                    new() { Id = "d", Label = "フルリモート希望" }
                },
                Deadline = DateTime.Now.AddDays(5)
            },
            new()
            {
                Id = "2",
                Question = "社内イベントの満足度調査",
                Type = SurveyType.Single,
                Deadline = DateTime.Now.AddDays(10),
                IsCompleted = true
            },
            new()
            {
                Id = "3",
                Question = "新オフィスレイアウトへのご意見",
                Type = SurveyType.Text,
                Deadline = DateTime.Now.AddDays(15)
            }
        };

        return Task.FromResult(surveys);
    }

    public Task SubmitAnswerAsync(string surveyId, string answer)
    {
        // TODO: 実際の実装では管理サーバーにPOST
        return Task.CompletedTask;
    }
}
