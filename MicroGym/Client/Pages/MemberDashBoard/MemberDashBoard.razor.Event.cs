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

        private void OnStatusChange(ChangeEventArgs e)
        {
            statusFilter = e.Value?.ToString() ?? string.Empty;
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
                    m.Status == statusFilter;

                return matchesSearch && matchesStatus;
            }).ToList();
        }
    }
}
