namespace MicroGym.Client.Pages.Attendance
{
    public partial class Attendance
    {
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            todayAttendance = await GetTodayAttendance();
            allMembers      = await GetMembers();

            isLoading = false;
        }
    }
}
