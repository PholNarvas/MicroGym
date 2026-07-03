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
        [Parameter] public int  UserId    { get; set; }   // 0 = nothing loaded yet
        [Parameter] public EventCallback OnClose         { get; set; }
        [Parameter] public EventCallback OnMemberUpdated { get; set; }

        // ── Injected Services ───────────────────────────────────
        [Inject] private MemberService             MemberService         { get; set; } = default!;
        [Inject] private MembershipTypeClientService MembershipTypeService { get; set; } = default!;

        // ── Internal State ──────────────────────────────────────
        private List<MembershipType> membershipTypes  = new();
        private EditMemberDto        model            = new();
        private string               editIsActiveString = string.Empty;

        private bool   isLoadingMember = false;
        private bool   isEditing       = false;
        private bool   editSuccess     = false;
        private string errorMessage    = string.Empty;

        // Track last loaded ID to avoid re-fetching on every render
        private int _lastLoadedUserId = 0;

        protected override async Task OnInitializedAsync()
        {
            membershipTypes = await MembershipTypeService.GetAllAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            // Only fetch when the modal opens with a new/different UserId
            if (IsOpen && UserId > 0 && UserId != _lastLoadedUserId)
            {
                _lastLoadedUserId = UserId;
                await LoadMember(UserId);
            }

            // Reset tracking when modal closes so next open always fetches fresh
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
                UserId          = member.UserId,
                FirstName       = member.FirstName,
                LastName        = member.LastName,
                Email           = member.Email,
                Phone           = member.Phone,
                MemberShipTypeID = member.MemberShipTypeID,
                // Inactive = renewal (blank payment), Active = pre-fill last payment
                Price           = member.IsActive ? member.Price         : null,
                PaymentMethod   = member.IsActive ? member.PaymentMethod : null
            };
            editIsActiveString = member.IsActive ? "true" : "false";
            isLoadingMember    = false;
        }

        // ── Event Handlers ───────────────────────────────────────

        private void OnMembershipTypeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var typeId))
            {
                model.MemberShipTypeID = typeId;
                var selected = membershipTypes.FirstOrDefault(mt => mt.MembershipTypeID == typeId);
                if (selected is not null)
                    model.Price = selected.Price;
            }
        }

        private async Task HandleSubmit()
        {
            isEditing    = true;
            errorMessage = string.Empty;

            model.IsActive = editIsActiveString == "true"  ? true
                           : editIsActiveString == "false" ? false
                           : null;

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

            isEditing = false;
        }

        private async Task HandleClose()
        {
            model              = new();
            editIsActiveString = string.Empty;
            errorMessage       = string.Empty;
            editSuccess        = false;
            _lastLoadedUserId  = 0;
            await OnClose.InvokeAsync();
        }
    }
}
