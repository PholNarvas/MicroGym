using MicroGym.Shared.DTOs;

namespace MicroGym.Client.Pages
{
    public partial class Register
    {
        private RegisterRequestDto registerModel = new();
        private string errorMessage = string.Empty;
        private bool isLoading = false;
        private bool isSuccess = false;
    }
}
