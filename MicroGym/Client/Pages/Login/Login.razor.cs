using MicroGym.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages
{
    public partial class Login
    {
        [SupplyParameterFromQuery] private string? returnUrl { get; set; }

        private LoginRequestDto loginModel = new();
        private LoginResponseDto loginRes;
        private string errorMessage = string.Empty;
        private bool isLoading = false;
        private string Valid { get; set; }
    }
}
