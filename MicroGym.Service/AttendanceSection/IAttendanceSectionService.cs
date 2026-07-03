using MicroGym.Shared.Model;

namespace MicroGym.Service.AttendanceSection
{
    public interface IAttendanceSectionService
    {
        Task<List<AttendanceModel>> GetTodayAttendanceAsync();
        Task<(bool Success, string Message)> CheckInMemberAsync(int userId);
    }
}
