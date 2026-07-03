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

        public async Task<User?> GetMemberByID(int id)
        {
            return await memberRepository.GetMemberByID(id);
        }

        public async Task<bool> SaveMember(EditMemberDto edit)
        {
            return await memberRepository.SaveMemberInfo(edit);
        }

        public async Task<bool> DeleteMember(int userID)
        {
            return await memberRepository.DeleteMember(userID);
        }
    }
}
