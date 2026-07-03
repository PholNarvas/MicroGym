using MicroGym.Shared.DTOs;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        // Adds a new member. Returns true if successful.
        public async Task<bool> AddMember(RegisterRequestDto model)
        {
            return await MemberService.AddMemberAsync(model);
        }

        // Saves edits to an existing member. Returns true if successful.
        public async Task<bool> SaveMember(EditMemberDto model)
        {
            return await MemberService.SaveMemberInfo(model);
        }
    }
}
