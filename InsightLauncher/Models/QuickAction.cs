namespace InsightLauncher.Models;

public enum QuickActionType
{
    Application,
    Url,
    Folder,
    File
}

public class QuickAction
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public QuickActionType Type { get; set; }
    public int Order { get; set; }
}
