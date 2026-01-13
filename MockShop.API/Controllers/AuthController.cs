using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockShop.Application.Interfaces;
using MockShop.Infrastructure.Persistance;

namespace MockShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context; // Login için basitçe context kullanalım
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            // 2. Password Check (Must be hashed but for now use real one)
            if (user == null || user.Password != request.Password)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            }

            // 3. Generate JWT Token
            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
