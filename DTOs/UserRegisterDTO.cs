namespace TodoAppBackend.Controllers
{
    public class UserRegisterDTO
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
