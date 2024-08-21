namespace TodoAppBackend.Controllers
{
    public class TasksDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public TaskState State { get; set; }
        public string[]? Type { get; set; }
        public string? ProjectName { get; set; }
        public int? ProjectId { get; set; }
        public int? AssignerId { get; set; }
        public int? AssigneeId { get; set; }
        public List<TagsDto> TaskTags { get; set; } = new List<TagsDto>();
        public List<CommentsDto> Comments { get; set; } = new List<CommentsDto>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public int? Order { get; set; }
    }
    public class TasksDtoForUpdate
    {
        public int Id { get; set; }
        public TaskState State { get; set; }
    }
    public class TaskOrderDto
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }

    public class TaskOrderUpdateDto
    {
        public List<TaskOrderDto> Tasks { get; set; }
    }
}