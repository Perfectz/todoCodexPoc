using System.Text.Json;
using System.IO;
using System.Linq;
using TodoCodexPoc.Models;

namespace TodoCodexPoc.Services;

public class FileTodoRepository : ITodoRepository, IDisposable
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
        
        // Ensure directory exists
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // Create empty file if it doesn't exist
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
        
        var filename = Path.GetFileName(_filePath);
        _watcher = new FileSystemWatcher(directory, filename)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
        };
        _watcher.Changed += OnFileChanged;
        _watcher.Created += OnFileChanged;
        _watcher.Deleted += OnFileChanged;
        _watcher.EnableRaisingEvents = true;
        
        // Initialize items list
        _items = new List<TodoItem>();
    }

    private async Task<List<TodoItem>> LoadAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            if (File.Exists(_filePath))
            {
                string json = await File.ReadAllTextAsync(_filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    _items = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
                }
                else
                {
                    _items = new List<TodoItem>();
                }
            }
            else
            {
                _items = new List<TodoItem>();
                File.WriteAllText(_filePath, "[]");
            }
            
            return _items;
        }
        finally
        {
            _mutex.Release();
        }
    }

    private async Task SaveAsync()
    {
        await _mutex.WaitAsync();
        try 
        {
            string json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
        finally
        {
            _mutex.Release();
            TasksChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken token = default)
    {
        var list = await LoadAsync();
        return list.Where(t => !t.IsArchived).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<TodoItem>> GetArchivedAsync(CancellationToken token = default)
    {
        var list = await LoadAsync();
        return list.Where(t => t.IsArchived).ToList().AsReadOnly();
    }

    public async Task<TodoItem?> GetAsync(Guid id, CancellationToken token = default)
    {
        var list = await LoadAsync();
        return list.FirstOrDefault(i => i.Id == id);
    }

    public async Task AddAsync(TodoItem item, CancellationToken token = default)
    {
        await _mutex.WaitAsync(token);
        try
        {
            await LoadAsync();
            _items!.Add(item);
            await SaveAsync();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task UpdateAsync(TodoItem item, CancellationToken token = default)
    {
        await _mutex.WaitAsync(token);
        try
        {
            await LoadAsync();
            var index = _items!.FindIndex(t => t.Id == item.Id);
            if (index >= 0)
            {
                _items[index] = item;
                await SaveAsync();
            }
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        await _mutex.WaitAsync(token);
        try
        {
            await LoadAsync();
            _items!.RemoveAll(t => t.Id == id);
            await SaveAsync();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task ArchiveAsync(Guid id, CancellationToken token = default)
    {
        await _mutex.WaitAsync(token);
        try
        {
            await LoadAsync();
            var index = _items!.FindIndex(t => t.Id == id);
            if (index >= 0)
            {
                _items![index].IsArchived = true;
                await SaveAsync();
            }
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task ToggleAsync(Guid id, CancellationToken token = default)
    {
        await _mutex.WaitAsync(token);
        try
        {
            await LoadAsync();
            var index = _items!.FindIndex(t => t.Id == id);
            if (index >= 0)
            {
                if (!_items[index].IsArchived)
                {
                    _items[index].IsDone = !_items[index].IsDone;
                    await SaveAsync();
                }
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

    public void Dispose()
    {
        _watcher.Dispose();
        _mutex.Dispose();
    }
}
