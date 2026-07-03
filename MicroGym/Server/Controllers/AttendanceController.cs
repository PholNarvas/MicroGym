using MicroGym.Service.AttendanceSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceSectionService     _attendanceSectionService;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(
            IAttendanceSectionService attendanceSectionService,
            ILogger<AttendanceController> logger)
        {
            _attendanceSectionService = attendanceSectionService;
            _logger                   = logger;
        }

        [HttpGet("GetTodayAttendance")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var attendance = await _attendanceSectionService.GetTodayAttendanceAsync();
            return Ok(attendance);
        }

        [HttpGet("GetAttendanceByDate")]
        public async Task<IActionResult> GetAttendanceByDate([FromQuery] DateTime date)
        {
            var attendance = await _attendanceSectionService.GetAttendanceByDateAsync(date);
            return Ok(attendance);
        }

        [HttpGet("GetWeeklyAttendanceSummary")]
        public async Task<IActionResult> GetWeeklyAttendanceSummary()
        {
            var summary = await _attendanceSectionService.GetWeeklyAttendanceSummaryAsync();
            return Ok(summary);
        }

        [HttpPost("CheckIn/{userId}")]
        public async Task<IActionResult> CheckIn(int userId)
        {
            var (success, message) = await _attendanceSectionService.CheckInMemberAsync(userId);

            if (!success)
            {
                _logger.LogWarning("Check-in failed for UserId {UserId}: {Message}", userId, message);
                return BadRequest(new { message });
            }

            _logger.LogInformation("Check-in successful for UserId {UserId}", userId);
            return Ok(new { message });
        }
    }
}
