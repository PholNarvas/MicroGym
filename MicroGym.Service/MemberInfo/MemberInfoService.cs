using MicroGym.Data.Repository.MemberRepo;
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

        public async Task<List<Members>> GetAllMembers()
        {
            return await memberRepository.GetAllMembers();
        }
    }
}
