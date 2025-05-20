using TodoCodexPoc.Models;

namespace TodoCodexPoc.Services;

public interface ITodoRepository : IDisposable
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken token = default);
    Task<IReadOnlyList<TodoItem>> GetArchivedAsync(CancellationToken token = default);
    Task<TodoItem?> GetAsync(Guid id, CancellationToken token = default);
    Task AddAsync(TodoItem item, CancellationToken token = default);
    Task UpdateAsync(TodoItem item, CancellationToken token = default);
    Task ToggleAsync(Guid id, CancellationToken token = default);
    Task DeleteAsync(Guid id, CancellationToken token = default);
    Task ArchiveAsync(Guid id, CancellationToken token = default);

    event EventHandler? TasksChanged;
}
