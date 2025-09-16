using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Taskify.Application.Interfaces;
using Taskify.Domain.Entities;

namespace Taskify.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User> RegisterAsync(string fullname, string email, string password)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        try
        {
            var user = new User
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow,
                FullName = fullname
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("A database error occurred while registering the user.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while registering the user.", ex);
        }
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;

        return user;
    }

    public async Task<User?> GoogleLoginAsync(string tokenId)
    {
        try
        {
            var clientId = _configuration["GoogleAuth:ClientId"];
            if (string.IsNullOrEmpty(clientId))
                throw new InvalidOperationException("Google ClientId not configured");

            var payload = await GoogleJsonWebSignature.ValidateAsync(tokenId, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            });

            if (payload?.Email == null)
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == payload.Email);
            return user;
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
            throw new InvalidOperationException("JWT configuration is missing");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwtIssuer,
            jwtIssuer,
            claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}