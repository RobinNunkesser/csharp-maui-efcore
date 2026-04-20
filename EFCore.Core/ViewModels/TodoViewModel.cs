using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TodoEF.Core.ViewModels;

public partial class TodoViewModel : ObservableObject
{
    private readonly ITodoRepository _repository;

    public ObservableCollection<TodoItem> Todos { get; } = [];

    [ObservableProperty]
    private string _newTitle = string.Empty;

    public TodoViewModel(ITodoRepository repository)
    {
        _repository = repository;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var items = await _repository.GetAllAsync();
        Todos.Clear();
        foreach (var item in items)
            Todos.Add(item);
    }

    [RelayCommand]
    public async Task AddAsync()
    {
        var title = NewTitle.Trim();
        if (string.IsNullOrEmpty(title)) return;

        var item = new TodoItem { Title = title };
        await _repository.SaveAsync(item);
        Todos.Add(item);
        NewTitle = string.Empty;
    }

    [RelayCommand]
    public async Task ToggleDoneAsync(TodoItem item)
    {
        item.Done = !item.Done;
        await _repository.SaveAsync(item);
    }

    [RelayCommand]
    public async Task DeleteAsync(TodoItem item)
    {
        await _repository.DeleteAsync(item);
        Todos.Remove(item);
    }
}
