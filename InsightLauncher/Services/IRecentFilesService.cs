using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface IRecentFilesService
{
    Task<List<RecentFile>> GetRecentFilesAsync();
    void OpenFile(string path);
    void OpenExplorer(string? path = null);
}
