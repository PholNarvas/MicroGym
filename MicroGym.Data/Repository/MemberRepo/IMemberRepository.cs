using MicroGym.Shared.Model;

namespace MicroGym.Data.Repository.MemberRepo
{
    public interface IMemberRepository
    {
        Task<List<Members>> GetAllMembers();
    }
}
