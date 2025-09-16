using Taskify.Domain.Enums;

namespace Taskify.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public TaskState State { get; set; }
    public string[] Type { get; set; } = Array.Empty<string>();
    public int Order { get; set; }

    // Foreign keys
    public int? ProjectId { get; set; }
    public int? AssignerId { get; set; }
    public int? AssigneeId { get; set; }

    // Navigation properties
    public Project? Project { get; set; }
    public User? Assigner { get; set; }
    public User? Assignee { get; set; }
    public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}