using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;
using System.Threading.Tasks;

namespace Secu_School_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly SchoolDbContext _context;

        public AuthService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(UserLoginDto loginDto)
        {
            try
            {
                var login = await _context.Logins
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName && u.PasswordHash == loginDto.Password);

                if (login == null) return null;

                return new User
                {
                    UserName = login.UserName,
                    UserType = login.UserType,
                    SchoolId = login.SchoolId
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                return null;
            }
            
        }
    }
}
