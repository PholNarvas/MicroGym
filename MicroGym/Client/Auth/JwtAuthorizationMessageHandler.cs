using System.Net.Http.Headers;

namespace MicroGym.Client.Auth
{
    public class JwtAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly JwtAuthStateProvider authStateProvider;

        public JwtAuthorizationMessageHandler(JwtAuthStateProvider authStateProvider)
        {
            this.authStateProvider = authStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await authStateProvider.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            // If the server rejects the token, clear the local session immediately.
            // CascadingAuthenticationState will pick up the state change and
            // RedirectToLogin (App.razor <NotAuthorized>) handles the navigation.
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                await authStateProvider.NotifyUserLogoutAsync();

            return response;
        }
    }
}
