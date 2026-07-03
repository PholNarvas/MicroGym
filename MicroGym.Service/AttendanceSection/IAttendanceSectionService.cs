using MicroGym.Shared.Model;

namespace MicroGym.Service.AttendanceSection
{
    public interface IAttendanceSectionService
    {
        Task<List<AttendanceModel>>      GetTodayAttendanceAsync();
        Task<List<AttendanceModel>>      GetAttendanceByDateAsync(DateTime date);
        Task<List<AttendanceDaySummary>> GetWeeklyAttendanceSummaryAsync();
        Task<(bool Success, string Message)> CheckInMemberAsync(int userId);
    }
}
