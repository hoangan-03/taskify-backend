namespace TodoAppBackend.Controllers
{
    public class EventsDtoForAdding
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public Color? Color { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Location { get; set; }
        public int? TaskId { get; set; }
        public int? CreatorId { get; set; }
        public List<EventUser> EventUsers { get; set; } = new List<EventUser>();
    }
    public class EventsDTOForUpdate
    {
        public int Id { get; set; }
        public TaskState State { get; set; }
    }
}