using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class QuickActionsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private ObservableCollection<QuickAction> _quickActions = new();

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _newActionName = string.Empty;

    [ObservableProperty]
    private string _newActionPath = string.Empty;

    [ObservableProperty]
    private string _newActionIcon = "🔗";

    [ObservableProperty]
    private QuickActionType _newActionType = QuickActionType.Url;

    public QuickActionsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        LoadQuickActions();
        _settingsService.SettingsChanged += (s, e) => LoadQuickActions();
    }

    [RelayCommand]
    private void ExecuteAction(QuickAction action)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true
            };

            switch (action.Type)
            {
                case QuickActionType.Url:
                    startInfo.FileName = action.Path;
                    break;
                case QuickActionType.Application:
                case QuickActionType.File:
                    startInfo.FileName = action.Path;
                    break;
                case QuickActionType.Folder:
                    startInfo.FileName = "explorer.exe";
                    startInfo.Arguments = action.Path;
                    break;
            }

            Process.Start(startInfo);
        }
        catch (Exception)
        {
            // エラー処理
        }
    }

    [RelayCommand]
    private void AddAction()
    {
        if (string.IsNullOrWhiteSpace(NewActionName) || string.IsNullOrWhiteSpace(NewActionPath))
            return;

        var action = new QuickAction
        {
            Name = NewActionName,
            Path = NewActionPath,
            Icon = NewActionIcon,
            Type = NewActionType
        };

        _settingsService.AddQuickAction(action);
        NewActionName = string.Empty;
        NewActionPath = string.Empty;
        NewActionIcon = "🔗";
        NewActionType = QuickActionType.Url;
        IsEditing = false;
        LoadQuickActions();
    }

    [RelayCommand]
    private void RemoveAction(QuickAction action)
    {
        _settingsService.RemoveQuickAction(action.Id);
        LoadQuickActions();
    }

    [RelayCommand]
    private void StartEditing()
    {
        IsEditing = true;
    }

    [RelayCommand]
    private void StopEditing()
    {
        IsEditing = false;
        NewActionName = string.Empty;
        NewActionPath = string.Empty;
    }

    private void LoadQuickActions()
    {
        QuickActions.Clear();
        foreach (var action in _settingsService.Settings.QuickActions.OrderBy(a => a.Order))
        {
            QuickActions.Add(action);
        }
    }
}
