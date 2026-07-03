
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
                // Get the member's name to show in the success popup
                var member = allMembers.FirstOrDefault(m => m.UserId == userId);
                checkedInMemberName = member is not null
                    ? $"{member.FirstName} {member.LastName}"
                    : "Member";

                // Open success popup and refresh the attendance log at the same time
                showSuccessAlert = true;
                todayAttendance  = await GetTodayAttendance();

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
