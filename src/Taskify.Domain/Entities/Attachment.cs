using Taskify.Domain.Enums;

namespace Taskify.Domain.Entities;

public class Attachment : BaseEntity
{
    public string Url { get; set; } = string.Empty;
    public string? Name { get; set; }
    public AttachmentType FileType { get; set; }
    public DateTime UploadedAt { get; set; }

    // Foreign key
    public int? TaskId { get; set; }

    // Navigation property
    public TaskItem? Task { get; set; }
}