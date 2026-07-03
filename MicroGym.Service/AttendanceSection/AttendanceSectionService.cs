using MicroGym.Data.Repository.AttendanceRepository;
using MicroGym.Shared.Model;
using Microsoft.Extensions.Caching.Memory;

namespace MicroGym.Service.AttendanceSection
{
    public class AttendanceSectionService : IAttendanceSectionService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMemoryCache           _cache;

        private const string WeeklySummaryCacheKey = "weekly_attendance_summary";

        public AttendanceSectionService(
            IAttendanceRepository attendanceRepository,
            IMemoryCache cache)
        {
            _attendanceRepository = attendanceRepository;
            _cache                = cache;
        }

        public async Task<List<AttendanceModel>> GetTodayAttendanceAsync()
        {
            return await _attendanceRepository.GetTodayAttendanceAsync();
        }

        public async Task<List<AttendanceModel>> GetAttendanceByDateAsync(DateTime date)
        {
            return await _attendanceRepository.GetAttendanceByDateAsync(date);
        }

        public async Task<List<AttendanceDaySummary>> GetWeeklyAttendanceSummaryAsync()
        {
            // Return cached result if available (valid for 5 minutes)
            if (_cache.TryGetValue(WeeklySummaryCacheKey, out List<AttendanceDaySummary>? cached) && cached is not null)
                return cached;

            var result = await _attendanceRepository.GetWeeklyAttendanceSummaryAsync();

            _cache.Set(WeeklySummaryCacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<(bool Success, string Message)> CheckInMemberAsync(int userId)
        {
            var isMembershipValid = await _attendanceRepository.IsMembershipValidAsync(userId);

            if (!isMembershipValid)
                return (false, "Membership is expired. Please renew before checking in.");

            var alreadyCheckedIn = await _attendanceRepository.IsAlreadyCheckedInAsync(userId);

            if (alreadyCheckedIn)
                return (false, "Member is already checked in today.");

            var success = await _attendanceRepository.CheckInMemberAsync(userId);

            if (success)
                _cache.Remove(WeeklySummaryCacheKey);   // Invalidate so the chart shows the new check-in

            return success
                ? (true, "Check-in successful.")
                : (false, "Check-in failed. Please try again.");
        }
    }
}
