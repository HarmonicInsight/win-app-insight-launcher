using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InsightLauncher.Models;
using InsightLauncher.Services;

namespace InsightLauncher.ViewModels;

public partial class TodoViewModel : ObservableObject
{
    private readonly ITodoService _todoService;

    [ObservableProperty]
    private ObservableCollection<TodoItem> _todos = new();

    [ObservableProperty]
    private string _newTodoTitle = string.Empty;

    [ObservableProperty]
    private DateTime? _newTodoDueDate;

    [ObservableProperty]
    private TodoPriority _newTodoPriority = TodoPriority.Medium;

    [ObservableProperty]
    private bool _showCompleted;

    [ObservableProperty]
    private bool _isAddingNew;

    public TodoViewModel(ITodoService todoService)
    {
        _todoService = todoService;
        _ = LoadTodosAsync();
    }

    public int PendingCount => Todos.Count(t => !t.IsCompleted);
    public int CompletedCount => Todos.Count(t => t.IsCompleted);

    public ObservableCollection<TodoItem> FilteredTodos =>
        new(Todos.Where(t => ShowCompleted || !t.IsCompleted));

    [RelayCommand]
    private async Task AddTodoAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTodoTitle))
            return;

        await _todoService.AddTodoAsync(NewTodoTitle, NewTodoDueDate, NewTodoPriority);
        NewTodoTitle = string.Empty;
        NewTodoDueDate = null;
        NewTodoPriority = TodoPriority.Medium;
        IsAddingNew = false;
        await LoadTodosAsync();
    }

    [RelayCommand]
    private async Task ToggleCompleteAsync(TodoItem todo)
    {
        await _todoService.ToggleCompleteAsync(todo.Id);
        await LoadTodosAsync();
    }

    [RelayCommand]
    private async Task DeleteTodoAsync(TodoItem todo)
    {
        await _todoService.DeleteTodoAsync(todo.Id);
        await LoadTodosAsync();
    }

    [RelayCommand]
    private void StartAddNew()
    {
        IsAddingNew = true;
    }

    [RelayCommand]
    private void CancelAddNew()
    {
        IsAddingNew = false;
        NewTodoTitle = string.Empty;
        NewTodoDueDate = null;
        NewTodoPriority = TodoPriority.Medium;
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadTodosAsync();
    }

    private async Task LoadTodosAsync()
    {
        var todos = await _todoService.GetTodosAsync();
        Todos.Clear();
        foreach (var todo in todos)
        {
            Todos.Add(todo);
        }
        OnPropertyChanged(nameof(FilteredTodos));
        OnPropertyChanged(nameof(PendingCount));
        OnPropertyChanged(nameof(CompletedCount));
    }

    partial void OnShowCompletedChanged(bool value)
    {
        OnPropertyChanged(nameof(FilteredTodos));
    }
}
