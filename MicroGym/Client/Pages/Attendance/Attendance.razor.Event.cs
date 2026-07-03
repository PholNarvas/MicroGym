using MicroGym.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace MicroGym.Client.Pages.Attendance
{
    public partial class Attendance
    {
        // Thin wrapper — looks up the member from the local list,
        // then delegates the actual validation logic to BaseApplication.
        private bool IsMembershipValid(int userId)
        {
            var member = allMembers.FirstOrDefault(x => x.UserId == userId);
            return IsMembershipValid(member);
        }

        private void OnSearchInput(ChangeEventArgs e)
        {
            searchText = e.Value?.ToString() ?? string.Empty;
            checkInMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchResults.Clear();
                return;
            }

            var query = searchText.Trim().ToLower();

            searchResults = allMembers
                .Where(m => m.FirstName.ToLower().Contains(query)
                         || m.LastName.ToLower().Contains(query)
                         || $"{m.FirstName} {m.LastName}".ToLower().Contains(query))
                .Take(8)
                .ToList();
        }

        private void ClearSearch()
        {
            searchText = string.Empty;
            checkInMessage = string.Empty;
            searchResults.Clear();
        }

        private void OpenAddModal()
        {
            modalSearchText = string.Empty;
            ModalService.Show(MemberPickerContent, "Select Member to Check In");
        }

        private void OnModalSearchInput(ChangeEventArgs e)
        {
            modalSearchText = e.Value?.ToString() ?? string.Empty;
        }

        private async Task OnModalCheckIn(int userId)
        {
            await OnCheckIn(userId);

            if (checkInSuccess)
                ModalService.Close();
        }

        private void CloseSuccessAlert()
        {
            showSuccessAlert = false;
        }

        // Called when the "Expired" button is clicked from the main search results.
        private void OnExpiredClick(User member)
        {
            expiredMemberName = $"{member.FirstName} {member.LastName}";
            expiredDate       = member.ExpiryDate;
            showExpiredAlert  = true;
        }

        // Called when the "Expired" button is clicked from inside the member picker modal.
        // Closes the modal first so the expired alert popup is visible behind it.
        private void OnModalExpiredClick(User member)
        {
            ModalService.Close();
            OnExpiredClick(member);
        }

        private void CloseExpiredAlert()
        {
            showExpiredAlert = false;
        }
    }
}
