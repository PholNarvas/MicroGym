using MicroGym.Client.Service;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class NavMenu
    {
        [Inject] private AuthClientService AuthClientService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private bool collapseNavMenu = true;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private async Task HandleLogout()
        {
            await AuthClientService.LogoutAsync();
            NavigationManager.NavigateTo("/login");
        }
    }
}
