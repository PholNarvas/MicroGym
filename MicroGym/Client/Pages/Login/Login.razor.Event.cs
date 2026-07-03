namespace MicroGym.Client.Pages
{
    public partial class Login
    {
        private async Task HandleLogin()
        {
            isLoading = true;
            errorMessage = string.Empty;
            loginRes = await AuthClientService.LoginAsync(loginModel);

            this.ValidateAccount();

            if (loginRes != null && loginRes.Role == "Admin")
            {
                var target = string.IsNullOrEmpty(returnUrl) ? "/" : Uri.UnescapeDataString(returnUrl);
                NavigationManager.NavigateTo(target);
            }
        }

        private void ValidateAccount()
        {
            if (loginRes == null)
            {
                errorMessage = "No Account Found.";
                isLoading = false;
                return;
            }

            if (loginRes.Role != "Admin")
            {
                errorMessage = "Access denied. Admins only.";
                isLoading = false;
                return;
            }

            isLoading = false;
        }
    }
}
