using TodoCodexPoc.Models;
using TodoCodexPoc.Services;

internal class Program
{
    private static async Task Main()
    {
        ITodoRepository repo = new FileTodoRepository();
        var items = await repo.GetAllAsync();

        if (!items.Any())
        {
            var sample = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = "Sample Task",
                DueDate = DateTimeOffset.UtcNow.AddDays(1),
                Priority = 1,
                IsDone = false
            };
            await repo.AddAsync(sample);
            Console.WriteLine($"Added sample item: {sample.Title}");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine($"- {item.Title} (done: {item.IsDone})");
            }
        }
    }
}
