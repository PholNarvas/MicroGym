using MicroGym.Service.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
    }
}
