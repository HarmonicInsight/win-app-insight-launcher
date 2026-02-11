using InsightLauncher.Models;

namespace InsightLauncher.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetTodosAsync();
    Task<TodoItem> AddTodoAsync(string title, DateTime? dueDate = null, TodoPriority priority = TodoPriority.Medium);
    Task UpdateTodoAsync(TodoItem todo);
    Task DeleteTodoAsync(string id);
    Task ToggleCompleteAsync(string id);
    int GetPendingCount();
}
