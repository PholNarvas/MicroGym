using MicroGym.Shared.Model;

namespace MicroGym.Service.MembershipTypeSection
{
    public interface IMembershipTypeService
    {
        Task<List<MembershipType>> GetAllMembershipTypes();
    }
}
