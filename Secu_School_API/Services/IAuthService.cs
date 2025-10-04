using System.Threading.Tasks;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(UserLoginDto dto);
    }
}
