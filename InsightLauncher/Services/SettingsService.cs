using System.IO;
using System.Text.Json;
using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class SettingsService : ISettingsService
{
    private readonly string _settingsPath;
    private UserSettings _settings = new();

    public UserSettings Settings => _settings;
    public event EventHandler? SettingsChanged;

    public SettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "InsightLauncher");
        Directory.CreateDirectory(appFolder);
        _settingsPath = Path.Combine(appFolder, "settings.json");
        Load();
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsPath, json);
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Load()
    {
        if (File.Exists(_settingsPath))
        {
            var json = File.ReadAllText(_settingsPath);
            _settings = JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
        }
        else
        {
            _settings = new UserSettings();
            InitializeDefaultQuickActions();
            Save();
        }
    }

    public void ToggleDarkMode()
    {
        _settings.IsDarkMode = !_settings.IsDarkMode;
        Save();
    }

    public void AddPinnedFile(string fileId)
    {
        if (!_settings.PinnedFileIds.Contains(fileId))
        {
            _settings.PinnedFileIds.Add(fileId);
            Save();
        }
    }

    public void RemovePinnedFile(string fileId)
    {
        if (_settings.PinnedFileIds.Remove(fileId))
        {
            Save();
        }
    }

    public bool IsFilePinned(string fileId) => _settings.PinnedFileIds.Contains(fileId);

    public void AddQuickAction(QuickAction action)
    {
        action.Order = _settings.QuickActions.Count;
        _settings.QuickActions.Add(action);
        Save();
    }

    public void RemoveQuickAction(string actionId)
    {
        var action = _settings.QuickActions.FirstOrDefault(a => a.Id == actionId);
        if (action != null)
        {
            _settings.QuickActions.Remove(action);
            Save();
        }
    }

    public void UpdateQuickAction(QuickAction action)
    {
        var index = _settings.QuickActions.FindIndex(a => a.Id == action.Id);
        if (index >= 0)
        {
            _settings.QuickActions[index] = action;
            Save();
        }
    }

    private void InitializeDefaultQuickActions()
    {
        _settings.QuickActions = new List<QuickAction>
        {
            new() { Name = "Outlook", Path = "https://outlook.office.com", Icon = "📧", Type = QuickActionType.Url, Order = 0 },
            new() { Name = "Teams", Path = "https://teams.microsoft.com", Icon = "💬", Type = QuickActionType.Url, Order = 1 },
            new() { Name = "SharePoint", Path = "https://www.office.com", Icon = "📁", Type = QuickActionType.Url, Order = 2 },
            new() { Name = "OneDrive", Path = "shell:OneDrive", Icon = "☁️", Type = QuickActionType.Folder, Order = 3 }
        };
    }
}
