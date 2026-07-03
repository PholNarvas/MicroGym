namespace MicroGym.Client.Pages.Attendance
{
    public partial class Attendance
    {
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;

                var todayTask   = GetTodayAttendance();
                var weeklyTask  = GetWeeklyAttendanceSummary();
                var membersTask = GetMembers();

                await Task.WhenAll(todayTask, weeklyTask, membersTask);

                todayAttendance = todayTask.Result;
                viewAttendance  = todayAttendance;
                weeklyData      = weeklyTask.Result;
                allMembers      = membersTask.Result;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception)
            {
                // Network failure or bad JSON — lists stay empty.
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
