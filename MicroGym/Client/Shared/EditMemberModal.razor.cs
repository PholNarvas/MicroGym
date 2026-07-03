using MicroGym.Client.Service;
using MicroGym.Client.Services;
using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class EditMemberModal
    {
        // ── Parameters ──────────────────────────────────────────
        [Parameter] public bool IsOpen    { get; set; }
        [Parameter] public int  UserId    { get; set; }
        [Parameter] public EventCallback OnClose         { get; set; }
        [Parameter] public EventCallback OnMemberUpdated { get; set; }

        // ── Injected Services ───────────────────────────────────
        [Inject] private MemberService               MemberService         { get; set; } = default!;
        [Inject] private MembershipTypeClientService MembershipTypeService { get; set; } = default!;

        // ── Internal State ──────────────────────────────────────
        private List<MembershipType> membershipTypes    = new();
        private EditMemberDto        model              = new();
        private string               editIsActiveString = string.Empty;

        private bool   isLoadingMember = false;
        private bool   isEditing       = false;
        private bool   editSuccess     = false;
        private string errorMessage    = string.Empty;

        private int _lastLoadedUserId = 0;

        protected override async Task OnInitializedAsync()
        {
            membershipTypes = await MembershipTypeService.GetAllAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (IsOpen && UserId > 0 && UserId != _lastLoadedUserId)
            {
                _lastLoadedUserId = UserId;
                await LoadMember(UserId);
            }

            if (!IsOpen)
                _lastLoadedUserId = 0;
        }

        // ── Private Helpers ─────────────────────────────────────

        private async Task LoadMember(int userId)
        {
            isLoadingMember = true;
            editSuccess     = false;
            errorMessage    = string.Empty;
            StateHasChanged();

            var member = await MemberService.GetMemberByIdAsync(userId);

            if (member is null)
            {
                errorMessage    = "Could not load member details. Please try again.";
                isLoadingMember = false;
                return;
            }

            model = new EditMemberDto
            {
                UserId           = member.UserId,
                FirstName        = member.FirstName,
                LastName         = member.LastName,
                Email            = member.Email,
                Phone            = member.Phone,
                MemberShipTypeID = member.MemberShipTypeID,
                IsActive         = member.IsActive
            };

            editIsActiveString = member.IsActive ? "true" : "false";
            isLoadingMember    = false;
        }

        // ── Event Handlers ───────────────────────────────────────

        private void OnMembershipTypeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var typeId))
                model.MemberShipTypeID = typeId;
        }

        private async Task HandleSubmit()
        {
            isEditing    = true;
            errorMessage = string.Empty;

            model.IsActive = editIsActiveString == "true"  ? true
                           : editIsActiveString == "false" ? false
                           : null;

            try
            {
                var success = await MemberService.SaveMemberInfo(model);

                if (success)
                {
                    editSuccess = true;
                    await OnMemberUpdated.InvokeAsync();
                }
                else
                {
                    errorMessage = "Failed to update member. Please try again.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                isEditing = false;
            }
        }

        private async Task HandleClose()
        {
            model              = new();
            editIsActiveString = string.Empty;
            errorMessage       = string.Empty;
            editSuccess        = false;
            isEditing          = false;
            _lastLoadedUserId  = 0;
            await OnClose.InvokeAsync();
        }
    }
}
