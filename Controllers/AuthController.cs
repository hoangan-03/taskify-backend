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

        public AuthController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserRegisterDTO userForRegisterDto)
        {
            if (await _authService.UserExists(userForRegisterDto.Email))
                return BadRequest("Email is already in use.");

            var user = await _authService.Register(userForRegisterDto.Email, userForRegisterDto.Password);

            return Ok(new UserDTO
            {
                Id = user.UserId ?? 0,
                Email = user.Email,
                Token = _authService.GenerateJwtToken(user)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginDto userForLoginDto)
        {
            var user = await _authService.Login(userForLoginDto.Email, userForLoginDto.Password);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new UserDTO
            {
                Id = user.UserId ?? 0,
                Email = user.Email,
                Token = _authService.GenerateJwtToken(user)
            });
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<UserDTO>> GoogleLogin(GoogleLoginDTO googleLoginDto)
        {
            var user = await _authService.GoogleLogin(googleLoginDto.TokenId);

            if (user == null)
                return Unauthorized("Google login failed.");

            return Ok(new UserDTO
            {
                Id = user.UserId ?? 0,
                Email = user.Email,
                Token = _authService.GenerateJwtToken(user)
            });
        }
    }
}