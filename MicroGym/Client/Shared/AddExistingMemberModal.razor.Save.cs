namespace MicroGym.Client.Shared
{
    public partial class AddExistingMemberModal
    {
        private async Task HandleSubmit()
        {
            isSaving     = true;
            errorMessage = string.Empty;

            var result = await MemberService.AddExistingMemberAsync(model);

            if (result == null)
            {
                errorMessage = "Email is already in use. Please try a different one.";
            }
            else if (result == true)
            {
                addSuccess = true;
                await OnMemberAdded.InvokeAsync();
            }
            else
            {
                errorMessage = "Something went wrong. Please try again.";
            }

            isSaving = false;
        }

        private async Task HandleClose()
        {
            model        = new();
            errorMessage = string.Empty;
            addSuccess   = false;
            isSaving     = false;
            await OnClose.InvokeAsync();
        }
    }
}
