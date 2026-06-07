namespace MicroGym.Client.Pages
{
    public partial class MemberDashBoard
    {
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            allMembers = await MemberService.GetMembersAsync();
            filteredMembers = allMembers;
            isLoading = false;
        }
    }
}
