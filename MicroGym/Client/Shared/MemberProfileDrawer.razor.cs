using MicroGym.Client.Service;
using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Shared
{
    public partial class MemberProfileDrawer
    {
        // ── Parameters ──────────────────────────────────────────
        [Parameter]
        public bool IsOpen { get; set; }
        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public EventCallback OnClose { get; set; }

        // Action callbacks — fire to parent to open the correct modal
        [Parameter]
        public EventCallback<User> OnEditRequested { get; set; }
        [Parameter]
        public EventCallback<User> OnRenewRequested { get; set; }
        [Parameter]
        public EventCallback<User> OnAnnualRequested { get; set; }
        [Parameter]
        public EventCallback<User> OnDeleteRequested { get; set; }

        // ── Injected Services ───────────────────────────────────
        [Inject]
        private MemberService MemberService { get; set; } = default!;

        // ── Internal State ──────────────────────────────────────
        private User? member = null;
        private List<MemberPaymentRecord> paymentHistory = new();
        private bool isLoading = false;
        private bool isUploadingPhoto = false;
        private int _lastLoadedUserId = 0;

    }
}
