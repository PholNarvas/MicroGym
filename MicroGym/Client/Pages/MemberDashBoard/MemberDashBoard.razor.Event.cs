using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages
{
    public partial class MemberDashBoard
    {
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

            filteredMembers = allMembers.Where(m =>
            {
                var matchesSearch = string.IsNullOrEmpty(search) ||
                    m.FirstName.ToLower().Contains(search) ||
                    m.LastName.ToLower().Contains(search) ||
                    m.Email.ToLower().Contains(search);

                var matchesStatus = string.IsNullOrEmpty(statusFilter) ||
                    (statusFilter == "Active" ? m.IsActive : !m.IsActive);

                return matchesSearch && matchesStatus;
            }).ToList();

            currentPage = 1;
        }

        private void OpenModal() => showAddModal = true;
        private void CloseAddModal() => showAddModal = false;

        private void OnEdit(User user)
        {
            editUserId = user.UserId;
            showEditModal = true;
        }

        private void CloseEditModal()
        {
            showEditModal = false;
            editUserId = 0;
        }

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
            {
                allMembers = await GetMembers();
                HandleSearch();
            }

            isDeleting = false;
            showDeleteModal = false;
            memberToDelete = null;
        }
    }
}
