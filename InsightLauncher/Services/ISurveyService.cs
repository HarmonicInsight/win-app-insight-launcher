using InsightLauncher.Models;

namespace InsightLauncher.Services;

/// <summary>
/// 集中管理サーバーからアンケートを取得・回答するサービス
/// </summary>
public interface ISurveyService
{
    Task<List<Survey>> GetSurveysAsync();
    Task SubmitAnswerAsync(string surveyId, string answer);
}
