using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface ISettingsService
{
    UserSettings Settings { get; }
    event EventHandler? SettingsChanged;

    void Save();
    void Load();
    void ToggleDarkMode();
    void AddPinnedFile(string fileId);
    void RemovePinnedFile(string fileId);
    bool IsFilePinned(string fileId);
    void AddQuickAction(QuickAction action);
    void RemoveQuickAction(string actionId);
    void UpdateQuickAction(QuickAction action);
}
