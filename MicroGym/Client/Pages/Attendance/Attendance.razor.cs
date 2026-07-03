using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.Attendance
{
    public partial class Attendance
    {
        private bool isLoading     = true;
        private bool isCheckingIn  = false;
        private bool checkInSuccess = false;

        private int    checkingInUserId = 0;
        private string searchText       = string.Empty;
        private string checkInMessage   = string.Empty;

        // ── Attendance Data ────────────────────────────────────
        private List<AttendanceModel>    todayAttendance  = new();   // always today (used for check-in validation)
        private List<AttendanceModel>    viewAttendance   = new();   // selected date (log + peak hours)
        private List<AttendanceDaySummary> weeklyData     = new();

        // ── Date Picker ────────────────────────────────────────
        private DateTime viewDate       = DateTime.Today;
        private bool     isViewLoading  = false;

        // ── Log Pagination ─────────────────────────────────────
        private int   logPage              = 1;
        private const int LogPageSize      = 10;
        private int   logTotalPages        => TotalPages(viewAttendance, LogPageSize);
        private List<AttendanceModel> pagedLog => GetPagedItems(viewAttendance, logPage, LogPageSize);

        private string ViewDateString
        {
            get => viewDate.ToString("yyyy-MM-dd");
            set { if (DateTime.TryParse(value, out var d)) viewDate = d; }
        }

        private bool IsViewingToday => viewDate.Date == DateTime.Today;

        // ── Members ────────────────────────────────────────────
        private List<User> allMembers   = new();
        private List<User> searchResults = new();

        // ── Peak Hours (computed from viewAttendance) ──────────
        private Dictionary<int, int> PeakHoursData =>
            viewAttendance
                .GroupBy(a => a.CheckInTime.Hour)
                .ToDictionary(g => g.Key, g => g.Count());

        private int PeakHoursMax =>
            PeakHoursData.Values.Any() ? PeakHoursData.Values.Max() : 1;

        private int WeeklyMax =>
            weeklyData.Any() ? weeklyData.Max(d => d.Count) : 1;

        private static string FormatHour(int h) =>
            h == 0  ? "12 AM" :
            h == 12 ? "12 PM" :
            h < 12  ? $"{h} AM" : $"{h - 12} PM";

        // ── Success Alert ──────────────────────────────────────
        private bool   showSuccessAlert    = false;
        private string checkedInMemberName = string.Empty;

        // ── Expired Alert ──────────────────────────────────────
        private bool      showExpiredAlert  = false;
        private string    expiredMemberName = string.Empty;
        private DateTime? expiredDate       = null;

        // ── Member Picker Modal ────────────────────────────────
        private bool   showMemberPickerModal = false;
        private string modalSearchText       = string.Empty;
        private CancellationTokenSource? _modalSearchCts;

        private List<User> ModalFilteredMembers =>
            allMembers
                .Where(m => MatchesMemberSearch(m, modalSearchText.Trim().ToLower()))
                .OrderBy(m => m.ExpiryStatus == "Expired")
                .ThenBy(m => m.LastName)
                .ToList();
    }
}
