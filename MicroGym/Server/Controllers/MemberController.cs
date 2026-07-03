using MicroGym.Service.MemberInfo;
using MicroGym.Shared.DTOs;
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

        [HttpGet("GetMemberById")]
        public async Task<IActionResult> GetMemberById(int memberID)
        {
            var member = await this.memberInfoService.GetMemberByID(memberID);
            if (member is null) return NotFound();
            return Ok(member);
        }

        [HttpPost("SaveMember")]
        public async Task<IActionResult> SaveMember([FromBody] EditMemberDto request)
        {
            var success = await memberInfoService.SaveMember(request);
            if (!success)
                return BadRequest("Email is already in use.");

            return Ok("Registration successful.");
        }

        [HttpDelete("DeleteMember/{userID}")]
        public async Task<IActionResult> DeleteMember(int userID)
        {
            var success = await memberInfoService.DeleteMember(userID);
            if (!success)
                return NotFound("Member not found.");

            return Ok("Member deleted successfully.");
        }
    }
}
