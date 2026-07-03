using Microsoft.AspNetCore.Components;
namespace MicroGym.Client.Pages.Revenues
{
    public partial class Revenue
    {
        private async Task OnMonthChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int month))
            {
                selectedMonth = month;
                await Initialize();
            }
        }

        private async Task OnYearChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int year))
            {
                selectedYear = year;
                await Initialize();
            }
        }
    }
}
