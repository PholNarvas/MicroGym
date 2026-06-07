using MicroGym.Shared.DTOs;

namespace MicroGym.Service.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<bool> RegisterAsync(RegisterRequestDto request);
    }
}
