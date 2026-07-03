using MicroGym.Shared.Model;

namespace MicroGym.Data.Repository.AttendanceRepository
{
    public interface IAttendanceRepository
    {
        Task<List<AttendanceModel>>    GetTodayAttendanceAsync();
        Task<List<AttendanceModel>>    GetAttendanceByDateAsync(DateTime date);
        Task<List<AttendanceDaySummary>> GetWeeklyAttendanceSummaryAsync();
        Task<bool> CheckInMemberAsync(int userId);
        Task<bool> IsAlreadyCheckedInAsync(int userId);
        Task<bool> IsMembershipValidAsync(int userId);
    }
}
