using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroGym.Client.Auth
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;

        private const string TokenKey           = "authToken";
        private const string LastActivityKey    = "lastActivity";
        private const int    InactivityMinutes  = 60;

        public JwtAuthStateProvider(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

            if (string.IsNullOrEmpty(token))
                return Unauthenticated();

            var claims = ParseClaimsFromJwt(token);

            // Check if JWT token itself is expired
            var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out var expSeconds))
            {
                var expiry = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                if (DateTime.UtcNow >= expiry)
                {
                    await ClearSessionDataAsync();
                    return Unauthenticated();
                }
            }

            // Check if user has been inactive for too long
            var lastActivityStr = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", LastActivityKey);
            if (!string.IsNullOrEmpty(lastActivityStr) && long.TryParse(lastActivityStr, out var lastActivityMs))
            {
                var lastActivity = DateTimeOffset.FromUnixTimeMilliseconds(lastActivityMs).UtcDateTime;
                if (DateTime.UtcNow - lastActivity > TimeSpan.FromMinutes(InactivityMinutes))
                {
                    await ClearSessionDataAsync();
                    return Unauthenticated();
                }
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        // Called by SessionMonitor timer every minute to proactively check the session
        public async Task CheckSessionAsync()
        {
            var token = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);

            // No token — already logged out, nothing to do
            if (string.IsNullOrEmpty(token)) return;

            var shouldLogout = false;
            var claims = ParseClaimsFromJwt(token);

            // Check JWT expiry
            var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out var expSeconds))
            {
                if (DateTime.UtcNow >= DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime)
                    shouldLogout = true;
            }

            // Check inactivity
            if (!shouldLogout)
            {
                var lastActivityStr = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", LastActivityKey);
                if (!string.IsNullOrEmpty(lastActivityStr) && long.TryParse(lastActivityStr, out var lastActivityMs))
                {
                    var lastActivity = DateTimeOffset.FromUnixTimeMilliseconds(lastActivityMs).UtcDateTime;
                    if (DateTime.UtcNow - lastActivity > TimeSpan.FromMinutes(InactivityMinutes))
                        shouldLogout = true;
                }
            }

            if (shouldLogout)
                await NotifyUserLogoutAsync();
        }

        public async Task NotifyUserLoginAsync(string token)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", LastActivityKey,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task NotifyUserLogoutAsync()
        {
            await ClearSessionDataAsync();
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        public async Task<string?> GetTokenAsync()
        {
            return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }

        private async Task ClearSessionDataAsync()
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", LastActivityKey);
        }

        private static AuthenticationState Unauthenticated()
            => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }
    }
}
