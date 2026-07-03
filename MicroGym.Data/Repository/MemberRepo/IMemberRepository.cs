using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Data.Repository.MemberRepo
{
    public interface IMemberRepository
    {
        Task<List<User>>                GetAllMembers();
        Task<User?>                     GetMemberByID(int memberid);
        Task<List<User>>                GetExpiringMembers();
        Task<bool>                      SaveMemberInfo(EditMemberDto member);
        Task<bool>                      RenewMember(RenewMemberDto dto);
        Task<bool>                      PurchaseAnnualMembership(PurchaseAnnualMembershipDto dto);
        Task<bool>                      DeleteMember(int userID);
        Task<List<MemberPaymentRecord>> GetMemberPaymentHistory(int userId);
        Task<bool>                      UpdateProfilePicture(UpdateProfilePictureDto dto);
        Task<List<MembershipTier>>      GetMembershipTiers();
        Task<int>                       AddExistingMember(AddExistingMemberDto dto, string passwordHash);
    }
}
