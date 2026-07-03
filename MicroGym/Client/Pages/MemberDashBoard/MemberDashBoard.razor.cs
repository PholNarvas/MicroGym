using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages
{
    public partial class MemberDashBoard
    {
        private List<User> allMembers = new();
        private List<User> filteredMembers = new();
        private bool isLoading = true;

        private string searchText = string.Empty;
        private string statusFilter = string.Empty;

        private int totalMembers    => allMembers.Count;
        private int activeMembers   => allMembers.Count(m => m.ExpiryStatus == "Active");
        private int inactiveMembers => allMembers.Count(m =>
            !m.ExpiryDate.HasValue
            || m.ExpiryDate.Value.Date < DateTime.Today
            || (m.TierExpiryDate.HasValue && m.TierExpiryDate.Value.Date < DateTime.Today));
        private int renewedToday    => allMembers.Count(m => m.LastPaymentDate.HasValue && m.LastPaymentDate.Value.Date == DateTime.Today);
        private int renewedThisWeek => allMembers.Count(m => m.LastPaymentDate.HasValue && m.LastPaymentDate.Value.Date >= DateTime.Today.AddDays(-7) && m.LastPaymentDate.Value.Date < DateTime.Today);

        // Add New Member Modal
        private bool showAddModal = false;

        // Add Existing Member Modal
        private bool showExistingModal = false;

        // Edit Member Modal
        private bool showEditModal = false;
        private int  editUserId   = 0;

        // Renew Member Modal
        private bool    showRenewModal       = false;
        private int     renewUserId          = 0;
        private string  renewMemberName      = string.Empty;
        private decimal renewTierDiscountPct = 0;

        // Annual Membership Modal
        private bool   showAnnualModal   = false;
        private int    annualUserId      = 0;
        private string annualMemberName  = string.Empty;

        // Delete Confirm Modal
        private bool showDeleteModal = false;
        private bool isDeleting = false;
        private User? memberToDelete = null;

        // Profile Drawer
        private bool showProfileDrawer = false;
        private int  profileUserId     = 0;

        // Sort — default: newest members first
        private string sortField     = "DateJoined";
        private bool   sortAscending = false;

        // Pagination
        private int currentPage = 1;
        private const int PageSize = 30;
        private int totalPages   => TotalPages(filteredMembers, PageSize);
        private List<User> pagedMembers => GetPagedItems(filteredMembers, currentPage, PageSize);
    }
}
