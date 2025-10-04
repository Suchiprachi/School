using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;
using Secu_School_API.Services;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly IAuthService _authService;  // Injected AuthService
        private readonly IJwtService _jwtService;    // Injected JwtService

        // Constructor with dependencies injected
        public LoginController(SchoolDbContext context, IAuthService authService, IJwtService jwtService)
        {
            _context = context;
            _authService = authService;
            _jwtService = jwtService;
        }

        // Login method
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            // Authenticate the user
            var user = await _authService.AuthenticateAsync(dto);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            // Return the user information and the token
            return Ok(new
            {
                token,
                user.UserName,
                user.UserType,
                user.SchoolId,
                user.EntityName,
                user.Photos
            });
        }
    }

}
