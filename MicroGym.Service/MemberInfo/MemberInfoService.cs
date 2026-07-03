using MicroGym.Data.Repository.MemberRepo;
using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Service.MemberInfo
{
    public class MemberInfoService : IMemberInfoService
    {
        private readonly IMemberRepository memberRepository;

        public MemberInfoService(IMemberRepository _memberRepository)
        {
            this.memberRepository = _memberRepository;
        }

        public async Task<List<User>> GetAllMembers()
        {
            return await memberRepository.GetAllMembers();
        }

        public async Task<List<User>> GetExpiringMembers()
        {
            return await memberRepository.GetExpiringMembers();
        }

        public async Task<User?> GetMemberByID(int id)
        {
            return await memberRepository.GetMemberByID(id);
        }

        public async Task<bool> SaveMember(EditMemberDto edit)
        {
            return await memberRepository.SaveMemberInfo(edit);
        }

        public async Task<bool> RenewMember(RenewMemberDto dto)
        {
            return await memberRepository.RenewMember(dto);
        }

        public async Task<bool> PurchaseAnnualMembership(PurchaseAnnualMembershipDto dto)
        {
            return await memberRepository.PurchaseAnnualMembership(dto);
        }

        public async Task<bool> DeleteMember(int userID)
        {
            return await memberRepository.DeleteMember(userID);
        }

        public async Task<List<MemberPaymentRecord>> GetMemberPaymentHistory(int userId)
        {
            return await memberRepository.GetMemberPaymentHistory(userId);
        }

        public async Task<bool> UpdateProfilePicture(UpdateProfilePictureDto dto)
        {
            return await memberRepository.UpdateProfilePicture(dto);
        }

        public async Task<List<MembershipTier>> GetMembershipTiers()
        {
            return await memberRepository.GetMembershipTiers();
        }

        public async Task<int> AddExistingMember(AddExistingMemberDto dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString());
            return await memberRepository.AddExistingMember(dto, passwordHash);
        }
    }
}
