namespace TodoMauiApp.Models;

public record TodoItem(int Id, string Title, DateTimeOffset? DueDate, bool IsDone, int Priority);
