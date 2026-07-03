using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        // Returns today's attendance log.
        public async Task<List<AttendanceModel>> GetTodayAttendance()
        {
            return await AttendanceClientService.GetTodayAttendanceAsync();
        }


    }
}
