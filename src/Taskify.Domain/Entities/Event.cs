using Taskify.Domain.Enums;

namespace Taskify.Domain.Entities;

public class Event : BaseEntity
{
    public string EventName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Color? Color { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Location { get; set; }

    // Foreign keys
    public int? TaskId { get; set; }
    public int? CreatorId { get; set; }

    // Navigation properties
    public TaskItem? Task { get; set; }
    public User? Creator { get; set; }
    public ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
}