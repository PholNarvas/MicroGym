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

        // Called on first load and after adding/renewing a member.
        private async Task LoadData()
        {
            try
            {
                isLoading  = true;
                expiryPage = 1;
                StateHasChanged();

                var membersTask    = GetMembers();
                var attendanceTask = GetTodayAttendance();
                var expiringTask   = GetExpiringMembers();

                await Task.WhenAll(membersTask, attendanceTask, expiringTask);

                totalMembers       = membersTask.Result.Count;
                totalActiveMembers = membersTask.Result.Count(x => x.ExpiryDate.HasValue && x.ExpiryDate.Value.Date >= DateTime.Today);
                todayCheckInCount  = attendanceTask.Result.Count;
                expiringMembers    = expiringTask.Result;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception)
            {
                // Network failure, server error, or malformed JSON — lists stay empty.
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
