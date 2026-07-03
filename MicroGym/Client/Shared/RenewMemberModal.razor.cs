using MicroGym.Client.Service;
using MicroGym.Client.Services;
using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class RenewMemberModal
    {
        // ── Parameters ──────────────────────────────────────────
        [Parameter] public bool   IsOpen        { get; set; }
        [Parameter] public int    UserId        { get; set; }
        [Parameter] public string MemberName    { get; set; } = string.Empty;
        [Parameter] public decimal TierDiscountPct { get; set; } = 0;
        [Parameter] public EventCallback OnClose         { get; set; }
        [Parameter] public EventCallback OnMemberRenewed { get; set; }

        // ── Injected Services ───────────────────────────────────
        [Inject] private MemberService               MemberService         { get; set; } = default!;
        [Inject] private MembershipTypeClientService MembershipTypeService { get; set; } = default!;

        // ── Internal State ──────────────────────────────────────
        private List<MembershipType> membershipTypes = new();
        private RenewMemberDto       model           = new();

        private bool   isRenewing    = false;
        private bool   renewSuccess  = false;
        private string errorMessage  = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            membershipTypes = await MembershipTypeService.GetAllAsync();
        }

        protected override void OnParametersSet()
        {
            // Pre-fill UserId whenever the modal opens with a member
            if (IsOpen && UserId > 0)
                model.UserId = UserId;
        }

        // ── Event Handlers ──────────────────────────────────────

        private void OnPlanChanged(ChangeEventArgs e)
        {
            if (!int.TryParse(e.Value?.ToString(), out var typeId)) return;

            model.MemberShipTypeID = typeId;

            // Auto-fill price — apply tier discount if member has an active paid tier
            var selected = membershipTypes.FirstOrDefault(mt => mt.MembershipTypeID == typeId);
            if (selected is not null)
                model.PaymentAmount = TierDiscountPct > 0
                    ? Math.Round(selected.Price * (1 - TierDiscountPct / 100), 2)
                    : selected.Price;
        }

        private async Task HandleSubmit()
        {
            isRenewing   = true;
            errorMessage = string.Empty;

            try
            {
                await MemberService.RenewMemberAsync(model);
                renewSuccess = true;
                await OnMemberRenewed.InvokeAsync();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                isRenewing = false;
            }
        }

        private async Task HandleClose()
        {
            model        = new();
            renewSuccess = false;
            errorMessage = string.Empty;
            isRenewing   = false;
            await OnClose.InvokeAsync();
        }
    }
}
