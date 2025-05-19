using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoCodexPoc.Models;
using TodoCodexPoc.Services;

namespace TaskTracker.Mobile.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly ITodoRepository _repo;

    public ObservableCollection<TodoItem> Tasks { get; } = new();

    private string _newTaskText = string.Empty;
    public string NewTaskText
    {
        get => _newTaskText;
        set => SetField(ref _newTaskText, value);
    }

    public ICommand AddTaskCommand { get; }
    public ICommand ToggleDoneCommand { get; }
    public ICommand DeleteTaskCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainPageViewModel(ITodoRepository repo)
    {
        _repo = repo;
        AddTaskCommand = new Command(async () => await AddTaskAsync());
        ToggleDoneCommand = new Command<TodoItem>(async item => await ToggleDoneAsync(item));
        DeleteTaskCommand = new Command<TodoItem>(async item => await DeleteTaskAsync(item));
        _repo.TasksChanged += async (s, e) => await OnTasksChanged(s, e);
        _ = LoadAsync();
    }

    private async Task OnTasksChanged(object? sender, EventArgs e)
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        var items = await _repo.GetAllAsync();
        Tasks.Clear();
        foreach (var item in items)
        {
            Tasks.Add(item);
        }
    }

    private async Task AddTaskAsync()
    {
        var text = NewTaskText?.Trim();
        if (string.IsNullOrEmpty(text))
            return;

        var item = new TodoItem { Id = Guid.NewGuid(), Title = text };
        await _repo.AddAsync(item);
        Tasks.Add(item);
        NewTaskText = string.Empty;
    }

    private async Task ToggleDoneAsync(TodoItem? item)
    {
        if (item == null)
            return;
        item.IsDone = !item.IsDone;
        await _repo.UpdateAsync(item);
    }

    private async Task DeleteTaskAsync(TodoItem? item)
    {
        if (item == null)
            return;
        await _repo.DeleteAsync(item.Id);
        Tasks.Remove(item);
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
