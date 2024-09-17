using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoAppBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAppBackend;

namespace TodoAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(ApplicationDbContext context, IAuthService authService, IEmailService emailService)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserRegisterDTO userForRegisterDto)
        {
            try
            {
                if (await _authService.UserExists(userForRegisterDto.Email))
                    return BadRequest("Email is already in use.");

                var user = await _authService.Register(userForRegisterDto.FullName,userForRegisterDto.Email, userForRegisterDto.Password);
                var emailBody = "<h1>Welcome to our platform!</h1><p>Thank you for registering.</p>";
                await _emailService.SendEmailAsync(userForRegisterDto.Email, "Welcome to Our Platform", emailBody);
                return Ok(new UserDTO
                {
                    FullName = user.FullName,
                    Id = user.UserId ?? 0,
                    Email = user.Email,
                    
                });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.Error.WriteLine($"Error in Register: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginDto userForLoginDto)
        {
            try
            {
                var user = await _authService.Login(userForLoginDto.Email, userForLoginDto.Password);

                if (user == null)
                    return Unauthorized("Invalid email or password.");

                return Ok(new UserDTO
                {
                    Id = user.UserId ?? 0,
                    Email = user.Email,
                });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.Error.WriteLine($"Error in Login: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<UserDTO>> GoogleLogin(GoogleLoginDTO googleLoginDto)
        {
            try
            {
                var user = await _authService.GoogleLogin(googleLoginDto.TokenId);

                if (user == null)
                    return Unauthorized("Google login failed.");

                return Ok(new UserDTO
                {
                    Id = user.UserId ?? 0,
                    Email = user.Email,
                });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.Error.WriteLine($"Error in GoogleLogin: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}