using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Data.Repository.MemberRepo
{
    public interface IMemberRepository
    {
        Task<List<User>> GetAllMembers();
        Task<User?> GetMemberByID(int memberid);
        Task<bool> SaveMemberInfo(EditMemberDto member);
        Task<bool> DeleteMember(int userID);
    }
}
