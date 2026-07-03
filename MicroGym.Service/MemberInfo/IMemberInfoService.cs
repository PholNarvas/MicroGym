using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Service.MemberInfo
{
    public interface IMemberInfoService
    {
        Task<List<User>> GetAllMembers();
        Task<User?> GetMemberByID(int id);
        Task<bool> SaveMember(EditMemberDto edit);
        Task<bool> DeleteMember(int userID);
    }
}
