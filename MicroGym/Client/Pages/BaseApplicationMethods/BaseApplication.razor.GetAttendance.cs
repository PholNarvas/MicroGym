using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        public async Task<List<AttendanceModel>> GetTodayAttendance()
        {
            return await AttendanceClientService.GetTodayAttendanceAsync();
        }

        public async Task<List<AttendanceModel>> GetAttendanceByDate(DateTime date)
        {
            return await AttendanceClientService.GetAttendanceByDateAsync(date);
        }

        public async Task<List<AttendanceDaySummary>> GetWeeklyAttendanceSummary()
        {
            return await AttendanceClientService.GetWeeklyAttendanceSummaryAsync();
        }
    }
}
