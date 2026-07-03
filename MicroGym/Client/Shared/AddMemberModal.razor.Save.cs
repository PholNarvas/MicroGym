namespace MicroGym.Client.Shared
{
    public partial class AddMemberModal
    {
        private async Task HandleSubmit()
        {
            isSaving     = true;
            errorMessage = string.Empty;

            var success = await MemberService.AddMemberAsync(model);

            if (success)
            {
                addSuccess = true;
                await OnMemberAdded.InvokeAsync();
            }
            else
            {
                errorMessage = "Email is already in use. Please try a different one.";
            }

            isSaving = false;
        }

        private async Task HandleClose()
        {
            model        = new();
            errorMessage = string.Empty;
            addSuccess   = false;
            isSaving     = false;
            SeedPassword();
            await OnClose.InvokeAsync();
        }
    }
}
