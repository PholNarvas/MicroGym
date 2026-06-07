using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MicroGym.Client.Pages
{
    public partial class Register
    {
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            // If already logged in, redirect to home
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated == true)
                NavigationManager.NavigateTo("/");
        }
    }
}
