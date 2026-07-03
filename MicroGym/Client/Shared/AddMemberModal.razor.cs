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
        [Parameter] public bool          IsOpen        { get; set; }
        [Parameter] public EventCallback OnClose       { get; set; }
        [Parameter] public EventCallback OnMemberAdded { get; set; }

        // ── Injected Services ───────────────────────────────────
        [Inject] private MemberService               MemberService         { get; set; } = default!;
        [Inject] private MembershipTypeClientService MembershipTypeService { get; set; } = default!;

        // ── Internal State ──────────────────────────────────────
        private List<MembershipType> membershipTypes = new();
        private List<MembershipTier> membershipTiers = new();
        private RegisterRequestDto   model           = new();

        private bool   isSaving     = false;
        private string errorMessage = string.Empty;
        private bool   addSuccess   = false;

        // The paid tier with a discount (Annual) — used for fee display and price calc
        private MembershipTier? annualTier => membershipTiers.FirstOrDefault(t => t.DiscountPct > 0 && t.IsActive);
        private bool            isAnnualSelected => model.TierID.HasValue && model.TierID == annualTier?.TierID;
    }
}
