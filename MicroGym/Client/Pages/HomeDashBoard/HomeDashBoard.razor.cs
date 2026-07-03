using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.HomeDashBoard
{
    public partial class HomeDashBoard
    {
        private bool isLoading = true;

        // ── User ────────────────────────────────────────────────
        private string currentUserName = string.Empty;

        // ── Counts ─────────────────────────────────────────────
        private int totalMembers         = 0;
        private int boxingTaekwondoCount = 0;
        private int gymMemberCount       = 0;
        private int zumbaMemberCount     = 0;
        private int todayCheckInCount    = 0;

        // ── Member Lists ────────────────────────────────────────
        private List<User>           boxingMembers   = new();
        private List<User>           gymMembers      = new();
        private List<User>           zumbaMembers    = new();
        private List<User>           expiringMembers = new();
        // ── Selected Category ───────────────────────────────────
        private int selectedCategory = 0;

        // ── Add Member Modal ────────────────────────────────────
        private bool showAddModal = false;

        // ── Helpers ─────────────────────────────────────────────
        private string GetGreeting()
        {
            var h = DateTime.Now.Hour;
            if (h < 12) return "Good morning";
            if (h < 17) return "Good afternoon";
            return "Good evening";
        }
    }
}
