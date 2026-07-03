using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    // Session monitoring is disabled for local machine deployment.
    // Token expiry is handled server-side via 401 responses caught
    // by JwtAuthorizationMessageHandler.
    public partial class SessionMonitor : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
