using MicroGym.Client.Service;
using MicroGym.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages
{
    public partial class Login
    {
        [Inject] private AuthClientService AuthClientService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [SupplyParameterFromQuery] private string? returnUrl { get; set; }

        private LoginRequestDto loginModel = new();
        private string errorMessage = string.Empty;
        private bool isLoading = false;
    }
}
