using System.Text.Json;
using TodoCodexPoc.Models;

namespace TodoCodexPoc.Services;

public class FileTodoRepository : ITodoRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _mutex = new(1, 1);
    private List<TodoItem>? _items;

    public FileTodoRepository(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(".", "data", "tasks.json");
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
                await SaveInternalAsync();
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

    private async Task SaveInternalAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        using FileStream stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, _items, new JsonSerializerOptions { WriteIndented = true });
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
            await SaveInternalAsync();
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
            await SaveInternalAsync();
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
            await SaveInternalAsync();
        }
        finally
        {
            _mutex.Release();
        }
    }
}
