using Taskify.Domain.Entities;

namespace Taskify.Application.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(string fullname, string email, string password);
    Task<User?> LoginAsync(string email, string password);
    Task<User?> GoogleLoginAsync(string tokenId);
    Task<bool> UserExistsAsync(string email);
    string GenerateJwtToken(User user);
}