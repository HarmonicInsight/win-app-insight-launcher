namespace InsightLauncher.Models;

public class RecentFile
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public FileType Type { get; set; }
    public DateTime LastModified { get; set; }

    /// <summary>
    /// フォルダパスを取得します（Office風表示用）
    /// </summary>
    public string FolderPath => System.IO.Path.GetDirectoryName(Path) ?? string.Empty;
}

public enum FileType
{
    Excel,
    Word,
    PowerPoint,
    Other
}
