using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.Attendance
{
    public partial class Attendance
    {
        private bool isLoading = true;
        private bool isCheckingIn = false;
        private bool checkInSuccess = false;

        private int checkingInUserId = 0;
        private string searchText = string.Empty;
        private string checkInMessage = string.Empty;

        // ── Data ───────────────────────────────────────────────
        private List<AttendanceModel> todayAttendance = new();
        private List<User> allMembers = new();
        private List<User> searchResults = new();

        // ── Success Alert ──────────────────────────────────────
        private bool   showSuccessAlert    = false;
        private string checkedInMemberName = string.Empty;

        // ── Expired Alert ──────────────────────────────────────
        private bool      showExpiredAlert  = false;
        private string    expiredMemberName = string.Empty;
        private DateTime? expiredDate       = null;

        // ── Modal ──────────────────────────────────────────────
        private string modalSearchText = string.Empty;
        private List<User> ModalFilteredMembers =>
            string.IsNullOrWhiteSpace(modalSearchText)
                ? allMembers
                : allMembers
                    .Where(m =>
                        m.FirstName.ToLower().Contains(modalSearchText.ToLower()) ||
                        m.LastName.ToLower().Contains(modalSearchText.ToLower()) ||
                        $"{m.FirstName} {m.LastName}".ToLower().Contains(modalSearchText.ToLower()))
                    .ToList();
    }
}
