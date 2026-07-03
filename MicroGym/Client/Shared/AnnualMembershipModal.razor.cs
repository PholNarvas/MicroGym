using MicroGym.Client.Service;
using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class AnnualMembershipModal
    {
        // ── Parameters ──────────────────────────────────────────
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public int UserId { get; set; }
        [Parameter] public string MemberName { get; set; } = string.Empty;
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnMemberActivated { get; set; }

        // ── Injected Services ───────────────────────────────────
        [Inject] private MemberService MemberService { get; set; } = default!;

        // ── Internal State ──────────────────────────────────────
        private List<MembershipTier> tiers = new();
        private MembershipTier? selectedTier = null;
        private PurchaseAnnualMembershipDto model = new();

        private bool isPurchasing = false;
        private bool purchaseSuccess = false;
        private string errorMessage = string.Empty;


    }
}
