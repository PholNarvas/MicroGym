using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroGym.Client.Auth
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;

        private const string TokenKey = "authToken";

        public JwtAuthStateProvider(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

            if (string.IsNullOrEmpty(token))
                return Unauthenticated();

            var claims  = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task NotifyUserLoginAsync(string token)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

            var claims  = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user    = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task NotifyUserLogoutAsync()
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        public async Task<string?> GetTokenAsync()
        {
            return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }

        private static AuthenticationState Unauthenticated()
            => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler  = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }
    }
}
