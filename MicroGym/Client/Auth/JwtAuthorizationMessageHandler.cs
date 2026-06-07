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

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
