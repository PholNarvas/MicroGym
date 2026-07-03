using MicroGym.Shared.Model;
using System.Reflection.Metadata;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        // Returns the full list of all members.
        public async Task<List<User>> GetMembers()
        {
            return await MemberService.GetMembersAsync();
        }

        // Returns a single member by their ID.
        public async Task<User?> GetMemberById(int userId)
        {
            return await MemberService.GetMemberByIdAsync(userId);
        }

        public async Task<List<User>> GetExpiringMembers()
        {
            return await MemberService.GetExpiringMemberAsync();
        }
    }
}
