namespace MicroGym.Client.Pages
{
    public partial class Login
    {
        protected override async Task OnInitializedAsync()
        {
            // If already logged in, redirect to home
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated == true)
                NavigationManager.NavigateTo("/");
        }
    }
}
