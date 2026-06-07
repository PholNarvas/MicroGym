using MicroGym.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    public partial class AuthController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result == null)
                return Unauthorized("Invalid email or password.");

            return Ok(result);
        }
    }
}
