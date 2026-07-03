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

        // Saves edits to an existing member's info. Returns true if successful.
        public async Task<bool> SaveMember(EditMemberDto model)
        {
            return await MemberService.SaveMemberInfo(model);
        }

        // Renews a member's subscription and records payment. Returns true if successful.
        public async Task<bool> RenewMember(RenewMemberDto model)
        {
            return await MemberService.RenewMemberAsync(model);
        }

        // Purchases Annual Membership for a member (₱500, valid 1 year). Returns true if successful.
        public async Task<bool> PurchaseAnnualMembership(PurchaseAnnualMembershipDto model)
        {
            return await MemberService.PurchaseAnnualMembershipAsync(model);
        }
    }
}
