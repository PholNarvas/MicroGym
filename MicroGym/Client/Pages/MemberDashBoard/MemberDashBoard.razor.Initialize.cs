namespace MicroGym.Client.Pages
{
    public partial class MemberDashBoard
    {
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;

                allMembers      = await GetMembers();
                filteredMembers = allMembers;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception)
            {
                // Network failure or bad JSON — list stays empty.
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
