using MicroGym.Client.Auth;
using MicroGym.Shared.DTOs;
using System.Net.Http.Json;

namespace MicroGym.Client.Service
{
    public class AuthClientService
    {
        private readonly HttpClient httpClient;
        private readonly JwtAuthStateProvider authStateProvider;

        public AuthClientService(HttpClient httpClient, JwtAuthStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (result == null) return null;

            await authStateProvider.NotifyUserLoginAsync(result.Token);
            return result;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            return response.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            await authStateProvider.NotifyUserLogoutAsync();
        }
    }
}
