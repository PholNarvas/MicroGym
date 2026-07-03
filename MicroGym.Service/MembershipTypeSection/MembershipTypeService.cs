using MicroGym.Data.Repository.MembershipTypeRepo;
using MicroGym.Shared.Model;

namespace MicroGym.Service.MembershipTypeSection
{
    public class MembershipTypeService : IMembershipTypeService
    {
        private readonly IMembershipTypeRepository membershipTypeRepository;

        public MembershipTypeService(IMembershipTypeRepository _membershipTypeRepository)
        {
            membershipTypeRepository = _membershipTypeRepository;
        }

        public async Task<List<MembershipType>> GetAllMembershipTypes()
        {
            return await membershipTypeRepository.GetAllMembershipTypes();
        }
    }
}
