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

        [HttpGet("GetExpiringMembers")]
        public async Task<IActionResult> GetExpiringMembers()
        {
            var expiringMember = await this.memberInfoService.GetExpiringMembers();
            return Ok(expiringMember);
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
            try
            {
                var success = await memberInfoService.SaveMember(request);
                if (!success)
                    return BadRequest("Member not found.");

                return Ok("Member updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RenewMember")]
        public async Task<IActionResult> RenewMember([FromBody] RenewMemberDto request)
        {
            try
            {
                await memberInfoService.RenewMember(request);
                return Ok("Member renewed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[RenewMember] Unhandled error: {ex}");
                return StatusCode(500, "An unexpected error occurred. Please try again.");
            }
        }

        [HttpPost("PurchaseAnnualMembership")]
        public async Task<IActionResult> PurchaseAnnualMembership([FromBody] PurchaseAnnualMembershipDto request)
        {
            try
            {
                await memberInfoService.PurchaseAnnualMembership(request);
                return Ok("Annual membership activated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                // Business logic failure — member not found, tier inactive, etc.
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Unexpected SQL or server error — log the real details, return generic message
                Console.Error.WriteLine($"[PurchaseAnnualMembership] Unhandled error: {ex}");
                return StatusCode(500, "An unexpected error occurred. Please try again.");
            }
        }

        [HttpDelete("DeleteMember/{userID}")]
        public async Task<IActionResult> DeleteMember(int userID)
        {
            var success = await memberInfoService.DeleteMember(userID);
            if (!success)
                return NotFound("Member not found.");

            return Ok("Member deleted successfully.");
        }

        [HttpGet("GetMemberPaymentHistory")]
        public async Task<IActionResult> GetMemberPaymentHistory(int memberID)
        {
            var history = await memberInfoService.GetMemberPaymentHistory(memberID);
            return Ok(history);
        }

        [HttpPost("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] UpdateProfilePictureDto request)
        {
            var success = await memberInfoService.UpdateProfilePicture(request);
            if (!success)
                return BadRequest("Failed to update profile picture.");

            return Ok("Profile picture updated.");
        }

        [HttpGet("GetMembershipTiers")]
        public async Task<IActionResult> GetMembershipTiers()
        {
            var tiers = await memberInfoService.GetMembershipTiers();
            return Ok(tiers);
        }

        [HttpPost("AddExistingMember")]
        public async Task<IActionResult> AddExistingMember([FromBody] AddExistingMemberDto request)
        {
            var newUserID = await memberInfoService.AddExistingMember(request);

            if (newUserID == -1)
                return Conflict("Email is already in use.");

            if (newUserID <= 0)
                return BadRequest("Failed to add existing member.");

            return Ok(newUserID);
        }
    }
}
