using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages
{
    public partial class MemberDashBoard
    {
        // ── Search & Filter ─────────────────────────────────────

        private void OnSearchInput(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? string.Empty;
            HandleSearch();
        }

        private void OnCardFilter(string filter)
        {
            statusFilter = filter;
            HandleSearch();
        }

        private void HandleSearch()
        {
            var search = searchText.Trim().ToLower();

            var query = allMembers.Where(m => MatchesSearch(m, search) && MatchesStatus(m));

            filteredMembers = ApplySort(query).ToList();
            currentPage     = 1;
        }

        private void OnSortFieldChange(ChangeEventArgs e)
        {
            sortField = e.Value?.ToString() ?? "Name";
            HandleSearch();
        }

        private void OnSortToggle()
        {
            sortAscending = !sortAscending;
            HandleSearch();
        }

        // ── Pagination ──────────────────────────────────────────

        private void OnPageChange(int page) => currentPage = page;

        // ── Shared Refresh ──────────────────────────────────────
        // Called by all modals after a successful add / edit / renew / annual purchase.

        private async Task OnMembersRefreshed()
        {
            allMembers = await GetMembers();
            HandleSearch();
        }

        // ── Modal: Add New Member ───────────────────────────────

        private void OpenModal()     => showAddModal = true;
        private void CloseAddModal() => showAddModal = false;

        // ── Modal: Add Existing Member ──────────────────────────

        private void OpenExistingModal()     => showExistingModal = true;
        private void CloseExistingModal()    => showExistingModal = false;

        // ── Modal: Edit ─────────────────────────────────────────

        private void OnEdit(User user)
        {
            editUserId    = user.UserId;
            showEditModal = true;
        }

        private void CloseEditModal()
        {
            showEditModal = false;
            editUserId    = 0;
        }

        // ── Modal: Renew ────────────────────────────────────────

        private void OnRenew(User user)
        {
            renewUserId          = user.UserId;
            renewMemberName      = $"{user.FirstName} {user.LastName}";
            renewTierDiscountPct = user.TierDiscountPct;
            showRenewModal       = true;
        }

        private void CloseRenewModal()
        {
            showRenewModal       = false;
            renewUserId          = 0;
            renewMemberName      = string.Empty;
            renewTierDiscountPct = 0;
        }

        // ── Modal: Annual Membership ────────────────────────────

        private void OnAnnual(User user)
        {
            annualUserId     = user.UserId;
            annualMemberName = $"{user.FirstName} {user.LastName}";
            showAnnualModal  = true;
        }

        private void CloseAnnualModal()
        {
            showAnnualModal  = false;
            annualUserId     = 0;
            annualMemberName = string.Empty;
        }

        // ── Modal: Delete ───────────────────────────────────────

        private void OnDelete(User user)
        {
            memberToDelete = user;
            showDeleteModal = true;
        }

        private void CloseDeleteModal()
        {
            memberToDelete = null;
            showDeleteModal = false;
        }

        private async Task ConfirmDelete()
        {
            if (memberToDelete is null) return;

            isDeleting = true;

            var success = await DeleteMember(memberToDelete.UserId);

            if (success)
                await OnMembersRefreshed();

            isDeleting      = false;
            showDeleteModal = false;
            memberToDelete  = null;
        }

        // ── Profile Drawer ──────────────────────────────────────

        private void OnRowClick(User user)
        {
            profileUserId     = user.UserId;
            showProfileDrawer = true;
        }

        private void CloseProfileDrawer()
        {
            showProfileDrawer = false;
            profileUserId     = 0;
        }
    }
}
