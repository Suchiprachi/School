using Secu_School_API.Models;

namespace Secu_School_API.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
