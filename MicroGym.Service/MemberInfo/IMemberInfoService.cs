using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Service.MemberInfo
{
    public interface IMemberInfoService
    {
        Task<List<User>>                GetAllMembers();
        Task<User?>                     GetMemberByID(int id);
        Task<List<User>>                GetExpiringMembers();
        Task<bool>                      SaveMember(EditMemberDto edit);
        Task<bool>                      RenewMember(RenewMemberDto dto);
        Task<bool>                      PurchaseAnnualMembership(PurchaseAnnualMembershipDto dto);
        Task<bool>                      DeleteMember(int userID);
        Task<List<MemberPaymentRecord>> GetMemberPaymentHistory(int userId);
        Task<bool>                      UpdateProfilePicture(UpdateProfilePictureDto dto);
        Task<List<MembershipTier>>      GetMembershipTiers();
        Task<int>                       AddExistingMember(AddExistingMemberDto dto);
    }
}
