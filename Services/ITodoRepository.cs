using TodoCodexPoc.Models;

namespace TodoCodexPoc.Services;

public interface ITodoRepository
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken token = default);
    Task<TodoItem?> GetAsync(Guid id, CancellationToken token = default);
    Task AddAsync(TodoItem item, CancellationToken token = default);
    Task UpdateAsync(TodoItem item, CancellationToken token = default);
    Task DeleteAsync(Guid id, CancellationToken token = default);
}
