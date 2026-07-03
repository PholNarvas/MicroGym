namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        // Deletes a member by their ID. Returns true if successful.
        public async Task<bool> DeleteMember(int userId)
        {
            return await MemberService.DeleteMemberAsync(userId);
        }
    }
}
