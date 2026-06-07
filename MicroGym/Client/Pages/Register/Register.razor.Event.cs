namespace MicroGym.Client.Pages
{
    public partial class Register
    {
        private async Task HandleRegister()
        {
            isLoading = true;
            errorMessage = string.Empty;

            var success = await AuthClientService.RegisterAsync(registerModel);

            if (success)
            {
                isSuccess = true;
            }
            else
            {
                errorMessage = "Email is already in use. Please try a different one.";
            }

            isLoading = false;
        }
    }
}
