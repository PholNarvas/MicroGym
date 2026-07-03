using MicroGym.Service.MembershipTypeSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipTypeController : ControllerBase
    {
        private readonly IMembershipTypeService membershipTypeService;

        public MembershipTypeController(IMembershipTypeService _membershipTypeService)
        {
            membershipTypeService = _membershipTypeService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var types = await membershipTypeService.GetAllMembershipTypes();
            return Ok(types);
        }
    }
}
