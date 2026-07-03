namespace MicroGym.Client.Pages.HomeDashBoard
{
    public partial class HomeDashBoard
    {
        protected override async Task OnInitializedAsync()
        {
            // Resolve username once — doesn't change during the session
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated == true)
            {
                var raw = authState.User.Identity.Name
                       ?? authState.User.FindFirst("email")?.Value
                       ?? string.Empty;
                if (raw.Contains('@')) raw = raw.Split('@')[0];
                currentUserName = raw.Length > 0
                    ? char.ToUpper(raw[0]) + raw[1..]
                    : string.Empty;
            }

            await LoadData();
        }

        // Called on first load and after adding a member to refresh KPI counts.
        private async Task LoadData()
        {
            isLoading = true;
            StateHasChanged();

            var membersTask    = GetMembers();
            var attendanceTask = GetTodayAttendance();

            await Task.WhenAll(membersTask, attendanceTask);

            var members    = membersTask.Result;
            var attendance = attendanceTask.Result;

            // ── Category splits ────────────────────────────────
            boxingMembers = members.Where(m => m.MemberShipTypeID == 1).ToList();
            gymMembers    = members.Where(m => m.MemberShipTypeID == 2).ToList();
            zumbaMembers  = members.Where(m => m.MemberShipTypeID == 3).ToList();

            totalMembers         = members.Count;
            boxingTaekwondoCount = boxingMembers.Count(m => m.IsActive);
            gymMemberCount       = gymMembers.Count(m => m.IsActive);
            zumbaMemberCount     = zumbaMembers.Count(m => m.IsActive);

            // ── Today's check-ins ──────────────────────────────
            todayCheckInCount = attendance.Count;

            // ── Expiring within 7 days ─────────────────────────
            var today   = DateTime.Today;
            var in7Days = today.AddDays(7);

            expiringMembers = members
                .Where(m => m.IsActive
                         && m.ExpiryDate.HasValue
                         && m.ExpiryDate.Value.Date >= today
                         && m.ExpiryDate.Value.Date <= in7Days)
                .OrderBy(m => m.ExpiryDate)
                .ToList();

            isLoading = false;
        }
    }
}
