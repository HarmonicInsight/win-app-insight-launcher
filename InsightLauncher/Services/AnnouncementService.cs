using InsightLauncher.Models;

namespace InsightLauncher.Services;

/// <summary>
/// 集中管理サーバーからお知らせを取得するサービス
/// 実際の実装では REST API を呼び出す
/// </summary>
public class AnnouncementService : IAnnouncementService
{
    private readonly IHttpClientFactory _httpClientFactory;
    // private const string ApiBaseUrl = "https://your-admin-server.com/api";

    public AnnouncementService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<List<Announcement>> GetAnnouncementsAsync()
    {
        // TODO: 実際の実装では管理サーバーから取得
        // var client = _httpClientFactory.CreateClient();
        // var response = await client.GetFromJsonAsync<List<Announcement>>($"{ApiBaseUrl}/announcements");

        // デモデータ
        var announcements = new List<Announcement>
        {
            new()
            {
                Id = "1",
                Type = AnnouncementType.Important,
                Title = "全社セキュリティ研修のお知らせ",
                Content = "2月10日(月)に全社セキュリティ研修があります。必ず参加してください。",
                Date = DateTime.Now.AddDays(-1),
                IsNew = true
            },
            new()
            {
                Id = "2",
                Type = AnnouncementType.Schedule,
                Title = "月例会議の日程変更",
                Content = "今月の月例会議は2月15日(土)から2月18日(火)に変更になりました。",
                Date = DateTime.Now.AddDays(-2),
                IsNew = true
            },
            new()
            {
                Id = "3",
                Type = AnnouncementType.Info,
                Title = "システムメンテナンスのお知らせ",
                Content = "2月8日(土) 深夜1時〜5時にシステムメンテナンスを実施します。",
                Date = DateTime.Now.AddDays(-3)
            },
            new()
            {
                Id = "4",
                Type = AnnouncementType.Info,
                Title = "新入社員歓迎会について",
                Content = "4月入社の新入社員歓迎会を企画中です。参加希望の方は人事部まで。",
                Date = DateTime.Now.AddDays(-5)
            }
        };

        return Task.FromResult(announcements);
    }

    public Task MarkAsReadAsync(string id)
    {
        // TODO: 実際の実装では管理サーバーにPOST
        return Task.CompletedTask;
    }
}
