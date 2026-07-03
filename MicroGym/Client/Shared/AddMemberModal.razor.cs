using MicroGym.Client.Service;
using MicroGym.Client.Services;
using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class AddMemberModal
    {
        // ── Parameters ──────────────────────────────────────────
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        /// Fired after a member is successfully saved.
        /// Parent uses this to refresh their member list / KPI counts.
        [Parameter] public EventCallback OnMemberAdded { get; set; }

        // ── Injected Services ───────────────────────────────────
        [Inject] private MemberService MemberService { get; set; } = default!;
        [Inject] private MembershipTypeClientService MembershipTypeService { get; set; } = default!;

        // ── Internal State ──────────────────────────────────────
        private List<MembershipType> membershipTypes = new();
        private RegisterRequestDto   model           = new();
        private bool   isSaving     = false;
        private string errorMessage  = string.Empty;
        private bool   addSuccess   = false;

        protected override async Task OnInitializedAsync()
        {
            membershipTypes = await MembershipTypeService.GetAllAsync();
        }

        // ── Handlers ────────────────────────────────────────────

        private void OnMembershipTypeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var typeId))
            {
                model.MemberShipTypeID = typeId;
                var selected = membershipTypes.FirstOrDefault(mt => mt.MembershipTypeID == typeId);
                if (selected is not null)
                    model.PaymentAmount = selected.Price;
            }
        }

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
            // Reset form state so the next open starts fresh
            model        = new();
            errorMessage = string.Empty;
            addSuccess   = false;
            await OnClose.InvokeAsync();
        }
    }
}
