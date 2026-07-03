using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages.HomeDashBoard
{
    public partial class HomeDashBoard
    {
        private void OnAlertSearchInput(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? string.Empty;
            expiryPage = 1;
        }

        private void OnToggleSort()
        {
            sortAscending = !sortAscending;
            expiryPage = 1;
        }

        private void OnExpiryPageChange(int page)
            => expiryPage = page;

        private void OpenAddModal()
            => showAddModal = true;
        private void CloseAddModal()
            => showAddModal = false;

        private async Task OnMemberAdded()
            => await LoadData();
    }
}
