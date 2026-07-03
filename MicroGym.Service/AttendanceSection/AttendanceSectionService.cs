using MicroGym.Data.Repository.AttendanceRepository;
using MicroGym.Shared.Model;

namespace MicroGym.Service.AttendanceSection
{
    public class AttendanceSectionService : IAttendanceSectionService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceSectionService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<List<AttendanceModel>> GetTodayAttendanceAsync()
        {
            return await _attendanceRepository.GetTodayAttendanceAsync();
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

            return success
                ? (true, "Check-in successful.")
                : (false, "Check-in failed. Please try again.");
        }
    }
}
