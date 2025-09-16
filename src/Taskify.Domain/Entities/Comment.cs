using Taskify.Domain.Enums;

namespace Taskify.Domain.Entities;

public class Comment : BaseEntity
{
    public CommentState State { get; set; }
    public string CommentText { get; set; } = string.Empty;
    public DateTime Timeline { get; set; }

    // Foreign keys
    public int? UserId { get; set; }
    public int? TaskId { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public TaskItem? Task { get; set; }
}