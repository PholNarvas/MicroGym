using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.BaseApplicationMethods
{
    public partial class BaseApplication
    {
        // Returns the full list of membership types (e.g. Gym, Boxing, Zumba).
        public async Task<List<MembershipType>> GetMembershipTypes()
        {
            return await MembershipTypeService.GetAllAsync();
        }
    }
}
