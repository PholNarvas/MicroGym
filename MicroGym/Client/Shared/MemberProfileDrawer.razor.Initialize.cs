using MicroGym.Shared.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace MicroGym.Client.Shared
{
    public partial class MemberProfileDrawer
    {
        protected override async Task OnParametersSetAsync()
        {
            if (IsOpen && UserId > 0 && UserId != _lastLoadedUserId)
            {
                _lastLoadedUserId = UserId;
                await LoadProfile(UserId);
            }

            if (!IsOpen)
                _lastLoadedUserId = 0;
        }

        // ── Private Helpers ─────────────────────────────────────

        private async Task LoadProfile(int userId)
        {
            try
            {
                isLoading = true;
                paymentHistory = new();
                StateHasChanged();

                member         = await MemberService.GetMemberByIdAsync(userId);
                paymentHistory = await MemberService.GetMemberPaymentHistoryAsync(userId);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Handler already cleared the session — let CascadingAuthenticationState redirect.
            }
            finally
            {
                isLoading = false;
            }
        }

        // ── Event Handlers ──────────────────────────────────────

        private async Task OnPhotoSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file is null || member is null) return;

            if (file.Size > 5 * 1024 * 1024) return;

            isUploadingPhoto = true;
            StateHasChanged();

            try
            {
                var resized = await file.RequestImageFileAsync("image/jpeg", 256, 256);
                using var ms = new MemoryStream();
                await resized.OpenReadStream(5 * 1024 * 1024).CopyToAsync(ms);
                var base64 = $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";

                var success = await MemberService.UpdateProfilePictureAsync(new UpdateProfilePictureDto
                {
                    UserId         = member.UserId,
                    ProfilePicture = base64
                });

                if (success)
                    member.ProfilePicture = base64;
            }
            catch (Exception)
            {
                // IO error or upload failure — spinner resets, photo unchanged.
            }
            finally
            {
                isUploadingPhoto = false;
            }
        }

        private async Task HandleClose()
        {
            member = null;
            paymentHistory = new();
            isLoading = false;
            _lastLoadedUserId = 0;
            await OnClose.InvokeAsync();
        }
    }
}
