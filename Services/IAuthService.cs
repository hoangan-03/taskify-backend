using System.Threading.Tasks;

namespace TodoAppBackend.Services
{
    public interface IAuthService
    {
        Task<User> Register(string fullname, string email, string password);
        Task<User> Login(string email, string password);
        Task<User> GoogleLogin(string tokenId);
        Task<bool> UserExists(string email);
        string GenerateJwtToken(User user);
    }
}