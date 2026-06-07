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

        public async Task<bool> LoginAsync(LoginRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (result == null) return false;

            await authStateProvider.NotifyUserLoginAsync(result.Token);
            return true;
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
