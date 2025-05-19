using System.Text.Json;
using System.IO;
using TodoCodexPoc.Models;

namespace TodoCodexPoc.Services;

public class FileTodoRepository : ITodoRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _mutex = new(1, 1);
    private readonly FileSystemWatcher _watcher;
    private List<TodoItem>? _items;

    public event EventHandler? TasksChanged;

    public FileTodoRepository(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(".", "data", "tasks.json");
        var directory = Path.GetDirectoryName(_filePath)!;
        var filename = Path.GetFileName(_filePath);
        _watcher = new FileSystemWatcher(directory, filename)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
        };
        _watcher.Changed += OnFileChanged;
        _watcher.Created += OnFileChanged;
        _watcher.Deleted += OnFileChanged;
        _watcher.EnableRaisingEvents = true;
    }

    private async Task<List<TodoItem>> LoadAsync()
    {
        if (_items != null)
        {
            return _items;
        }

        await _mutex.WaitAsync();
        try
        {
            if (_items != null)
            {
                return _items;
            }

            if (!File.Exists(_filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
                _items = new List<TodoItem>();
                await SaveAsync();
            }
            else
            {
                using FileStream stream = File.OpenRead(_filePath);
                _items = await JsonSerializer.DeserializeAsync<List<TodoItem>>(stream) ?? new List<TodoItem>();
            }
        }
        finally
        {
            _mutex.Release();
        }

        return _items;
    }

    private async Task SaveAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        using FileStream stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, _items, new JsonSerializerOptions { WriteIndented = true });
        TasksChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken token = default)
    {
        var list = await LoadAsync();
        return list.AsReadOnly();
    }

    public async Task<TodoItem?> GetAsync(Guid id, CancellationToken token = default)
    {
        var list = await LoadAsync();
        return list.FirstOrDefault(i => i.Id == id);
    }

    public async Task AddAsync(TodoItem item, CancellationToken token = default)
    {
        var list = await LoadAsync();
        await _mutex.WaitAsync(token);
        try
        {
            list.Add(item);
            await SaveAsync();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task UpdateAsync(TodoItem item, CancellationToken token = default)
    {
        var list = await LoadAsync();
        await _mutex.WaitAsync(token);
        try
        {
            var index = list.FindIndex(t => t.Id == item.Id);
            if (index >= 0)
            {
                list[index] = item;
            }
            await SaveAsync();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var list = await LoadAsync();
        await _mutex.WaitAsync(token);
        try
        {
            list.RemoveAll(t => t.Id == id);
            await SaveAsync();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task ToggleAsync(Guid id, CancellationToken token = default)
    {
        var list = await LoadAsync();
        await _mutex.WaitAsync(token);
        try
        {
            var index = list.FindIndex(t => t.Id == id);
            if (index >= 0)
            {
                list[index].IsDone = !list[index].IsDone;
                await SaveAsync();
            }
        }
        finally
        {
            _mutex.Release();
        }
    }

    private void OnFileChanged(object? sender, FileSystemEventArgs e)
    {
        _items = null;
        TasksChanged?.Invoke(this, EventArgs.Empty);
    }
}
