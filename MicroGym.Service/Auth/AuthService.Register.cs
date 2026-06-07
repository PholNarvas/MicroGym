using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Service.Auth
{
    public partial class AuthService
    {
        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Member",
                CreatedAt = DateTime.UtcNow
            };

            var (success, _) = await userRepository.RegisterAsync(user);
            return success;
        }
    }
}
