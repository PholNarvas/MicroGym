using MicroGym.Service.AttendanceSection;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceSectionService _attendanceSectionService;

        public AttendanceController(IAttendanceSectionService attendanceSectionService)
        {
            _attendanceSectionService = attendanceSectionService;
        }

        [HttpGet("GetTodayAttendance")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var attendance = await _attendanceSectionService.GetTodayAttendanceAsync();
            return Ok(attendance);
        }

        [HttpPost("CheckIn/{userId}")]
        public async Task<IActionResult> CheckIn(int userId)
        {
            var (success, message) = await _attendanceSectionService.CheckInMemberAsync(userId);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }
    }
}
