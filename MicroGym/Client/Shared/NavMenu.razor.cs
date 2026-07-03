using MicroGym.Client.Service;
using MicroGym.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MicroGym.Client.Shared
{
    public partial class NavMenu
    {
        [Inject] private AuthClientService           AuthClientService { get; set; } = default!;
        [Inject] private NavigationManager           NavigationManager { get; set; } = default!;
        [Inject] private ThemeService                ThemeService      { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        private bool   collapseNavMenu = true;
        private string userName        = string.Empty;
        private string userInitials    = "MG";

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated != true) return;

            // Get the raw name or email from the JWT
            var raw = authState.User.Identity.Name
                   ?? authState.User.FindFirst("email")?.Value
                   ?? "User";

            // If it is an email address, use only the part before @
            if (raw.Contains('@'))
                raw = raw.Split('@')[0];

            userName = raw;

            // Build up to 2-letter initials
            var parts = raw.Split(new[] { ' ', '.', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            userInitials = parts.Length >= 2
                ? $"{parts[0][0]}{parts[1][0]}".ToUpper()
                : (raw.Length >= 2 ? raw[..2].ToUpper() : raw.ToUpper());
        }

        private void ToggleNavMenu() => collapseNavMenu = !collapseNavMenu;

        private void ToggleTheme() => ThemeService.ToggleTheme();

        private async Task HandleLogout()
        {
            await AuthClientService.LogoutAsync();
            NavigationManager.NavigateTo("/login");
        }
    }
}
