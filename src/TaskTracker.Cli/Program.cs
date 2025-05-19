using TodoCodexPoc.Models;
using TodoCodexPoc.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        if (args.Length > 0 && args[0].Equals("voice", StringComparison.OrdinalIgnoreCase))
        {
            IVoiceToText voiceService = new DummyVoiceToText();
            var text = await voiceService.TranscribeAsync();
            Console.WriteLine(text);
            return;
        }

        using ITodoRepository repo = new FileTodoRepository();

        if (args.Length > 0 && args[0].Equals("add", StringComparison.OrdinalIgnoreCase))
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: add \"task description\"");
                return;
            }

            string text = string.Join(' ', args.Skip(1));
            DateTimeOffset? due = TextDateParser.Parse(ref text);

            var item = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = text,
                DueDate = due,
                Priority = 1,
                IsDone = false
            };
            await repo.AddAsync(item);
            Console.WriteLine($"Added task: {item.Title} {(item.DueDate.HasValue ? item.DueDate.Value.ToString() : string.Empty)}");
            return;
        }

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
