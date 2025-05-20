namespace TodoCodexPoc.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset? DueDate { get; set; }
    public bool IsDone { get; set; }
    public int Priority { get; set; }
    public bool IsArchived { get; set; }
}
