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

        // ── Inline Search (page) ────────────────────────────────

        private void OnSearchInput(ChangeEventArgs e)
        {
            searchText     = e.Value?.ToString() ?? string.Empty;
            checkInMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchResults.Clear();
                return;
            }

            var search = searchText.Trim().ToLower();
            searchResults = allMembers
                .Where(m => MatchesMemberSearch(m, search))
                .Take(8)
                .ToList();
        }

        private void ClearSearch()
        {
            searchText     = string.Empty;
            checkInMessage = string.Empty;
            searchResults.Clear();
        }

        // ── Date Picker ─────────────────────────────────────────

        private async Task OnDateChanged(ChangeEventArgs e)
        {
            if (!DateTime.TryParse(e.Value?.ToString(), out var date)) return;

            viewDate      = date;
            isViewLoading = true;
            StateHasChanged();

            viewAttendance = IsViewingToday
                ? todayAttendance
                : await GetAttendanceByDate(viewDate);

            logPage       = 1;
            isViewLoading = false;
        }

        private async Task OnResetToToday()
        {
            await OnDateChanged(new ChangeEventArgs { Value = DateTime.Today.ToString("yyyy-MM-dd") });
        }

        // ── Pagination ───────────────────────────────────────────

        private void OnLogPageChange(int page) => logPage = page;

        // ── Member Picker Modal ──────────────────────────────────

        private void OpenAddModal()
        {
            modalSearchText       = string.Empty;
            showMemberPickerModal = true;
        }

        private void ClosePickerModal()
        {
            showMemberPickerModal = false;
            modalSearchText       = string.Empty;
        }

        private async Task OnModalSearchInput(ChangeEventArgs e)
        {
            var text = e.Value?.ToString() ?? string.Empty;

            // Cancel the previous pending search so rapid keystrokes
            // don't trigger a re-render on every character.
            _modalSearchCts?.Cancel();
            _modalSearchCts = new CancellationTokenSource();

            try
            {
                await Task.Delay(180, _modalSearchCts.Token);
                modalSearchText = text;
            }
            catch (TaskCanceledException) { }
        }

        private async Task OnModalCheckIn(int userId)
        {
            await OnCheckIn(userId);

            if (checkInSuccess)
                ClosePickerModal();
        }

        // ── Expired Alert ────────────────────────────────────────

        private void OnExpiredClick(User member)
        {
            expiredMemberName = $"{member.FirstName} {member.LastName}";
            expiredDate       = member.ExpiryDate;
            showExpiredAlert  = true;
        }

        // Closes the picker first so the expired alert is visible on top.
        private void OnModalExpiredClick(User member)
        {
            ClosePickerModal();
            OnExpiredClick(member);
        }

        private void CloseExpiredAlert() => showExpiredAlert = false;

        // ── Success Alert ────────────────────────────────────────

        private void CloseSuccessAlert() => showSuccessAlert = false;
    }
}
