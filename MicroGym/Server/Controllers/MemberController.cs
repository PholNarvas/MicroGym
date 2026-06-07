using MicroGym.Service.MemberInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberInfoService memberInfoService;
        public MemberController(IMemberInfoService _memberInfoService)
        {
            this.memberInfoService = _memberInfoService;
        }

        [HttpGet("GetMembers")]
        public async Task<IActionResult> GetMembers()
        {
            var memberList = await this.memberInfoService.GetAllMembers();
            return Ok(memberList);
        }

    }
}
