using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface ISearchService
{
    Task<IEnumerable<SearchResult>> SearchAsync(string query);
}
