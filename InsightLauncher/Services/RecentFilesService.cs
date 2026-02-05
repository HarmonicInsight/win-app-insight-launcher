using System.Diagnostics;
using System.IO;
using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class RecentFilesService : IRecentFilesService
{
    public Task<List<RecentFile>> GetRecentFilesAsync()
    {
        var recentFiles = new List<RecentFile>();

        try
        {
            var recentPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Microsoft", "Windows", "Recent"
            );

            if (Directory.Exists(recentPath))
            {
                var files = Directory.GetFiles(recentPath, "*.lnk")
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .Take(20);

                foreach (var file in files)
                {
                    var name = Path.GetFileNameWithoutExtension(file);
                    var fileType = GetFileType(name);

                    // Office系ファイルのみ対象
                    if (fileType != FileType.Other)
                    {
                        recentFiles.Add(new RecentFile
                        {
                            Name = name,
                            Path = file,
                            Type = fileType,
                            LastModified = File.GetLastWriteTime(file)
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error reading recent files: {ex.Message}");
        }

        // デモデータを追加（実際のファイルがない場合）
        if (recentFiles.Count == 0)
        {
            recentFiles = GetDemoFiles();
        }

        return Task.FromResult(recentFiles);
    }

    private static FileType GetFileType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".xlsx" or ".xls" or ".xlsm" => FileType.Excel,
            ".docx" or ".doc" => FileType.Word,
            ".pptx" or ".ppt" => FileType.PowerPoint,
            _ => FileType.Other
        };
    }

    public void OpenFile(string path)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error opening file: {ex.Message}");
        }
    }

    public void OpenExplorer(string? path = null)
    {
        try
        {
            var targetPath = path ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = targetPath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error opening explorer: {ex.Message}");
        }
    }

    private static List<RecentFile> GetDemoFiles()
    {
        var now = DateTime.Now;
        return new List<RecentFile>
        {
            new() { Name = "2026年度予算計画.xlsx", Path = @"C:\Users\Documents\2026年度予算計画.xlsx", Type = FileType.Excel, LastModified = now.AddHours(-1) },
            new() { Name = "週次報告書_Week5.docx", Path = @"C:\Users\Documents\週次報告書_Week5.docx", Type = FileType.Word, LastModified = now.AddHours(-2) },
            new() { Name = "プロジェクト進捗報告.pptx", Path = @"C:\Users\Documents\プロジェクト進捗報告.pptx", Type = FileType.PowerPoint, LastModified = now.AddDays(-1) },
            new() { Name = "売上データ_1月.xlsx", Path = @"C:\Users\Documents\売上データ_1月.xlsx", Type = FileType.Excel, LastModified = now.AddDays(-1) },
            new() { Name = "会議議事録_20260204.docx", Path = @"C:\Users\Documents\会議議事録_20260204.docx", Type = FileType.Word, LastModified = now.AddDays(-2) },
            new() { Name = "新製品企画書.pptx", Path = @"C:\Users\Documents\新製品企画書.pptx", Type = FileType.PowerPoint, LastModified = now.AddDays(-2) },
            new() { Name = "顧客リスト_2026.xlsx", Path = @"C:\Users\Documents\顧客リスト_2026.xlsx", Type = FileType.Excel, LastModified = now.AddDays(-3) },
            new() { Name = "業務マニュアル.docx", Path = @"C:\Users\Documents\業務マニュアル.docx", Type = FileType.Word, LastModified = now.AddDays(-4) },
        };
    }
}
