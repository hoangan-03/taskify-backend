namespace TodoAppBackend.Controllers
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
    }
}
