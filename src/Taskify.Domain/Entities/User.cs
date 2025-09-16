namespace Taskify.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<EventUser> EventUsers { get; set; } = new List<EventUser>();
    public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
}