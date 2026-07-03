namespace MicroGym.Client.Shared
{
    public partial class AddExistingMemberModal
    {
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var plansTask = MembershipTypeService.GetAllAsync();
                var tiersTask = MemberService.GetMembershipTiersAsync();

                await Task.WhenAll(plansTask, tiersTask);

                membershipTypes = plansTask.Result;
                membershipTiers = tiersTask.Result;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Handler already cleared session — component will unmount on redirect.
            }
        }
    }
}
