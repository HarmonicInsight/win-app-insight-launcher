using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class TeamService : ITeamService
{
    public bool IsConnected { get; private set; }

    public Task ConnectAsync()
    {
        // 実際の実装ではMicrosoft Graph APIやSlack APIに接続
        IsConnected = true;
        return Task.CompletedTask;
    }

    public Task<IEnumerable<TeamMember>> GetTeamMembersAsync()
    {
        // モックデータ（実際はAPIから取得）
        var members = new List<TeamMember>
        {
            new() { Id = "1", Name = "田中 太郎", Email = "tanaka@example.com", Status = PresenceStatus.Available, StatusMessage = "作業中" },
            new() { Id = "2", Name = "鈴木 花子", Email = "suzuki@example.com", Status = PresenceStatus.Busy, StatusMessage = "会議中" },
            new() { Id = "3", Name = "佐藤 次郎", Email = "sato@example.com", Status = PresenceStatus.Away },
            new() { Id = "4", Name = "高橋 美咲", Email = "takahashi@example.com", Status = PresenceStatus.DoNotDisturb, StatusMessage = "集中作業中" },
            new() { Id = "5", Name = "伊藤 健太", Email = "ito@example.com", Status = PresenceStatus.Offline }
        };
        return Task.FromResult<IEnumerable<TeamMember>>(members);
    }

    public Task UpdateStatusAsync(PresenceStatus status, string? message = null)
    {
        // 実際の実装ではAPIを通じてステータスを更新
        return Task.CompletedTask;
    }
}
