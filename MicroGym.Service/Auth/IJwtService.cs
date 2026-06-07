using MicroGym.Shared.Model;

namespace MicroGym.Service.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
