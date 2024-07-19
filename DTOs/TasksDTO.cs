namespace TodoAppBackend.DTOs
{
    public class TasksDTO
    {
        public string? Title { get; set; }
        public string? Project { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string? Type { get; set; }
        public Tag? Tag { get; set; }
        public List<Attachment>? Attachments { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}