using MicroGym.Shared.Model;

namespace MicroGym.Service.MemberInfo
{
    public interface IMemberInfoService
    {
        Task<List<Members>> GetAllMembers();
    }
}
