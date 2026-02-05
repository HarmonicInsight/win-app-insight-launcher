namespace InsightLauncher.Models;

public class RecentFile
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public FileType Type { get; set; }
    public DateTime LastModified { get; set; }
}

public enum FileType
{
    Excel,
    Word,
    PowerPoint,
    Other
}
