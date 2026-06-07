using MicroGym.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    public partial class AuthController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var success = await authService.RegisterAsync(request);
            if (!success)
                return BadRequest("Email is already in use.");

            return Ok("Registration successful.");
        }
    }
}
