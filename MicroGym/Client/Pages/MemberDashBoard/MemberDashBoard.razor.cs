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

        private int totalMembers => allMembers.Count;
        private int activeMembers => allMembers.Count(m => m.IsActive);
        private int inactiveMembers => allMembers.Count(m => !m.IsActive);

        // Add Member Modal
        private bool showAddModal = false;

        // Edit Member Modal
        private bool showEditModal = false;
        private int editUserId = 0;

        // Delete Confirm Modal
        private bool showDeleteModal = false;
        private bool isDeleting = false;
        private User? memberToDelete = null;

        // Pagination
        private int currentPage = 1;
        private const int PageSize = 30;
        private int totalPages => (int)Math.Ceiling(filteredMembers.Count / (double)PageSize);
        private List<User> pagedMembers => filteredMembers
            .Skip((currentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }
}
