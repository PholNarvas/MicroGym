
namespace MicroGym.Client.Pages.Attendance
{
    public partial class Attendance
    {
        private async Task OnCheckIn(int userId)
        {
            isCheckingIn     = true;
            checkingInUserId = userId;
            checkInMessage   = string.Empty;

            var (success, message) = await CheckInMember(userId);

            checkInSuccess = success;

            if (success)
            {
                var member = allMembers.FirstOrDefault(m => m.UserId == userId);
                checkedInMemberName = member is not null
                    ? $"{member.FirstName} {member.LastName}"
                    : "Member";

                // Refresh all data that depends on today's check-ins
                todayAttendance = await GetTodayAttendance();
                weeklyData      = await GetWeeklyAttendanceSummary();

                // Sync the log view if we're still on today
                if (IsViewingToday)
                {
                    viewAttendance = todayAttendance;
                    logPage        = 1;
                }

                showSuccessAlert = true;
                StateHasChanged();

                // Auto-close the popup after 3 seconds
                _ = Task.Delay(3000).ContinueWith(_ =>
                {
                    showSuccessAlert = false;
                    InvokeAsync(StateHasChanged);
                });
            }
            else
            {
                checkInMessage = message;
            }

            isCheckingIn     = false;
            checkingInUserId = 0;
        }
    }
}
