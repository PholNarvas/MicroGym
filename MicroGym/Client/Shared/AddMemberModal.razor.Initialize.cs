namespace MicroGym.Client.Shared
{
    public partial class AddMemberModal
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

                SeedPassword();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Handler already cleared session — component will unmount on redirect.
            }
        }

        private void SeedPassword()
        {
            var random            = Guid.NewGuid().ToString();
            model.Password        = random;
            model.ConfirmPassword = random;
        }
    }
}
