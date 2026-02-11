using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface ITeamService
{
    Task<IEnumerable<TeamMember>> GetTeamMembersAsync();
    Task UpdateStatusAsync(PresenceStatus status, string? message = null);
    bool IsConnected { get; }
    Task ConnectAsync();
}
