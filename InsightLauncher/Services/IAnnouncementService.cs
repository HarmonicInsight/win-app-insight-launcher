using InsightLauncher.Models;

namespace InsightLauncher.Services;

/// <summary>
/// 集中管理サーバーからお知らせを取得するサービス
/// </summary>
public interface IAnnouncementService
{
    Task<List<Announcement>> GetAnnouncementsAsync();
    Task MarkAsReadAsync(string id);
}
