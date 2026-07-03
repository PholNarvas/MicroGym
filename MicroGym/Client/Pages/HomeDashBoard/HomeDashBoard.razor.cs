using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.HomeDashBoard
{
    public partial class HomeDashBoard
    {
        private bool isLoading = true;

        // ── User ────────────────────────────────────────────────
        private string currentUserName = string.Empty;

        // ── Counts ─────────────────────────────────────────────
        private int totalMembers       = 0;
        private int totalActiveMembers = 0;
        private int todayCheckInCount  = 0;

        // ── Member Lists ────────────────────────────────────────
        private List<User> expiringMembers = new();

        // ── Search & Sort ────────────────────────────────────────
        private string searchText    = string.Empty;
        private bool   sortAscending = true;  // true = soonest expiry first (most urgent)

        private List<User> displayedMembers
        {
            get
            {
                var search = searchText.Trim().ToLower();
                var query  = expiringMembers.Where(m => MatchesAlertSearch(m, search));
                return sortAscending
                    ? query.OrderBy(m => m.ExpiryDate ?? DateTime.MaxValue).ToList()
                    : query.OrderByDescending(m => m.ExpiryDate ?? DateTime.MinValue).ToList();
            }
        }

        // ── Expiring Members Pagination ─────────────────────────
        private int            expiryPage       = 1;
        private const int      ExpiryPageSize   = 30;
        private int            expiryTotalPages => TotalPages(displayedMembers, ExpiryPageSize);
        private List<User>     pagedExpiring    => GetPagedItems(displayedMembers, expiryPage, ExpiryPageSize);

        // ── Add Member Modal ────────────────────────────────────
        private bool showAddModal = false;
    }
}
