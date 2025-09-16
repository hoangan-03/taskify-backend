namespace Taskify.Domain.Entities;

public class Tag : BaseEntity
{
    public string TagName { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
}