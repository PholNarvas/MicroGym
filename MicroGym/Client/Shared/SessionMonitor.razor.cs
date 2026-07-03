using MicroGym.Client.Auth;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class SessionMonitor : IAsyncDisposable
    {
        [Inject] private JwtAuthStateProvider AuthStateProvider { get; set; } = default!;

        private PeriodicTimer? _timer;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return Task.CompletedTask;

            _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            // Fire-and-forget the timer loop — runs in background
            _ = RunTimerLoopAsync();

            return Task.CompletedTask;
        }

        private async Task RunTimerLoopAsync()
        {
            if (_timer is null) return;

            while (await _timer.WaitForNextTickAsync())
            {
                await AuthStateProvider.CheckSessionAsync();
            }
        }

        public ValueTask DisposeAsync()
        {
            _timer?.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
