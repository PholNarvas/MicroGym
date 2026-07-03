namespace MicroGym.Client.Shared
{
    public partial class AnnualMembershipModal
    {
        protected override async Task OnParametersSetAsync()
        {
            if (!IsOpen || UserId <= 0) return;

            model.UserId = UserId;

            if (tiers.Count == 0)
            {
                try
                {
                    tiers        = await MemberService.GetMembershipTiersAsync();
                    selectedTier = tiers.FirstOrDefault(t => t.Fee > 0);
                    model.TierID = selectedTier?.TierID ?? 0;
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Handler already cleared session — component will unmount on redirect.
                }
            }
        }

        private async Task HandleSubmit()
        {
            isPurchasing = true;
            errorMessage = string.Empty;

            try
            {
                await MemberService.PurchaseAnnualMembershipAsync(model);
                purchaseSuccess = true;
                await OnMemberActivated.InvokeAsync();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                isPurchasing = false;
            }
        }

        private async Task HandleClose()
        {
            model           = new();
            tiers           = new();
            selectedTier    = null;
            purchaseSuccess = false;
            errorMessage    = string.Empty;
            isPurchasing    = false;
            await OnClose.InvokeAsync();
        }
    }
}
