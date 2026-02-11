using System.IO;
using System.Text.Json;
using InsightLauncher.Models;

namespace InsightLauncher.Services;

public class TodoService : ITodoService
{
    private readonly string _todosPath;
    private List<TodoItem> _todos = new();

    public TodoService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "InsightLauncher");
        Directory.CreateDirectory(appFolder);
        _todosPath = Path.Combine(appFolder, "todos.json");
        LoadTodos();
    }

    public Task<IEnumerable<TodoItem>> GetTodosAsync()
    {
        return Task.FromResult<IEnumerable<TodoItem>>(
            _todos.OrderByDescending(t => t.Priority)
                  .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
                  .ThenBy(t => t.CreatedAt));
    }

    public Task<TodoItem> AddTodoAsync(string title, DateTime? dueDate = null, TodoPriority priority = TodoPriority.Medium)
    {
        var todo = new TodoItem
        {
            Title = title,
            DueDate = dueDate,
            Priority = priority
        };
        _todos.Add(todo);
        SaveTodos();
        return Task.FromResult(todo);
    }

    public Task UpdateTodoAsync(TodoItem todo)
    {
        var index = _todos.FindIndex(t => t.Id == todo.Id);
        if (index >= 0)
        {
            _todos[index] = todo;
            SaveTodos();
        }
        return Task.CompletedTask;
    }

    public Task DeleteTodoAsync(string id)
    {
        _todos.RemoveAll(t => t.Id == id);
        SaveTodos();
        return Task.CompletedTask;
    }

    public Task ToggleCompleteAsync(string id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo != null)
        {
            todo.IsCompleted = !todo.IsCompleted;
            SaveTodos();
        }
        return Task.CompletedTask;
    }

    public int GetPendingCount() => _todos.Count(t => !t.IsCompleted);

    private void LoadTodos()
    {
        if (File.Exists(_todosPath))
        {
            var json = File.ReadAllText(_todosPath);
            _todos = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
        }
    }

    private void SaveTodos()
    {
        var json = JsonSerializer.Serialize(_todos, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_todosPath, json);
    }
}
