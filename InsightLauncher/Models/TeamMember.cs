namespace InsightLauncher.Models;

public enum PresenceStatus
{
    Available,
    Busy,
    Away,
    DoNotDisturb,
    Offline
}

public class TeamMember
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public PresenceStatus Status { get; set; } = PresenceStatus.Offline;
    public string? StatusMessage { get; set; }
}
