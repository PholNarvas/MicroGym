using MicroGym.Client.Service;
using MicroGym.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages
{
    public partial class Register
    {
        [Inject] private AuthClientService AuthClientService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private RegisterRequestDto registerModel = new();
        private string errorMessage = string.Empty;
        private bool isLoading = false;
        private bool isSuccess = false;
    }
}
