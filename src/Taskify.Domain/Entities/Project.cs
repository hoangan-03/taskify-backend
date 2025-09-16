namespace Taskify.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}