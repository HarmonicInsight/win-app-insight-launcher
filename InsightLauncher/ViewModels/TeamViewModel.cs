using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class TeamViewModel : ObservableObject
{
    private readonly ITeamService _teamService;

    [ObservableProperty]
    private ObservableCollection<TeamMember> _teamMembers = new();

    [ObservableProperty]
    private bool _isConnected;

    [ObservableProperty]
    private bool _isLoading;

    public TeamViewModel(ITeamService teamService)
    {
        _teamService = teamService;
        IsConnected = _teamService.IsConnected;
        if (IsConnected)
        {
            _ = LoadTeamMembersAsync();
        }
    }

    public int OnlineCount => TeamMembers.Count(m => m.Status != PresenceStatus.Offline);

    [RelayCommand]
    private async Task ConnectAsync()
    {
        IsLoading = true;
        try
        {
            await _teamService.ConnectAsync();
            IsConnected = _teamService.IsConnected;
            await LoadTeamMembersAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadTeamMembersAsync();
    }

    private async Task LoadTeamMembersAsync()
    {
        IsLoading = true;
        try
        {
            var members = await _teamService.GetTeamMembersAsync();
            TeamMembers.Clear();
            foreach (var member in members.OrderBy(m => m.Status == PresenceStatus.Offline))
            {
                TeamMembers.Add(member);
            }
            OnPropertyChanged(nameof(OnlineCount));
        }
        finally
        {
            IsLoading = false;
        }
    }

    public static string GetStatusIcon(PresenceStatus status) => status switch
    {
        PresenceStatus.Available => "🟢",
        PresenceStatus.Busy => "🔴",
        PresenceStatus.Away => "🟡",
        PresenceStatus.DoNotDisturb => "⛔",
        PresenceStatus.Offline => "⚫",
        _ => "⚫"
    };

    public static string GetStatusText(PresenceStatus status) => status switch
    {
        PresenceStatus.Available => "連絡可能",
        PresenceStatus.Busy => "取り込み中",
        PresenceStatus.Away => "退席中",
        PresenceStatus.DoNotDisturb => "応答不可",
        PresenceStatus.Offline => "オフライン",
        _ => "不明"
    };
}
