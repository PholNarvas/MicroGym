namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        // Submits a check-in for the given member.
        // Returns (Success: bool, Message: string).
        public async Task<(bool Success, string Message)> CheckInMember(int userId)
        {
            return await AttendanceClientService.CheckInMemberAsync(userId);
        }
    }
}
