namespace MicroGym.Client.Shared
{
    public partial class MemberProfileDrawer
    {
        private async Task HandleEdit()
        {
            if (member is null) return;
            await OnEditRequested.InvokeAsync(member);
        }

        private async Task HandleRenew()
        {
            if (member is null) return;
            await OnRenewRequested.InvokeAsync(member);
        }

        private async Task HandleAnnual()
        {
            if (member is null) return;
            await OnAnnualRequested.InvokeAsync(member);
        }

        private async Task HandleDelete()
        {
            if (member is null) return;
            await OnDeleteRequested.InvokeAsync(member);
        }
    }
}
