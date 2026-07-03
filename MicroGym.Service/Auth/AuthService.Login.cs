using MicroGym.Shared.DTOs;

namespace MicroGym.Service.Auth
{
    public partial class AuthService
    {
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            var token = jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
