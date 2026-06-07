namespace MicroGym.Client.Pages
{
    public partial class Login
    {
        private async Task HandleLogin()
        {
            isLoading = true;
            errorMessage = string.Empty;

            var success = await AuthClientService.LoginAsync(loginModel);

            if (success)
            {
                var target = string.IsNullOrEmpty(returnUrl) ? "/" : Uri.UnescapeDataString(returnUrl);
                NavigationManager.NavigateTo(target);
            }
            else
            {
                errorMessage = "Invalid email or password.";
                isLoading = false;
            }
        }
    }
}
