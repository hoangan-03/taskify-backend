namespace TodoAppBackend.Controllers
{
    public class ProjectDTO
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreateAt { get; set; }
    }
    
}