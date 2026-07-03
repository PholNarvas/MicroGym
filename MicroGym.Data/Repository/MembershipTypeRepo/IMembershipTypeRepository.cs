using MicroGym.Shared.Model;

namespace MicroGym.Data.Repository.MembershipTypeRepo
{
    public interface IMembershipTypeRepository
    {
        Task<List<MembershipType>> GetAllMembershipTypes();
    }
}
