using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace MicroGym.Client.Pages.Revenues
{
    public partial class Revenue
    {
        // ── PIN Lock ───────────────────────────────────────────
        private async Task OnUnlockRevenue()
        {
            if (pinInput == RevenuePIN)
            {
                isRevenueLocked = false;
                pinError = string.Empty;
                await Initialize();
            }
            else
            {
                pinError = "Incorrect PIN. Please try again.";
                pinInput = string.Empty;
            }
        }

        private async Task OnPinKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
                await OnUnlockRevenue();
        }

        // ──────────────────────────────────────────────────────
        private async Task OnMonthChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int month))
            {
                selectedMonth = month;
                txPage = 1;
                await Initialize();
            }
        }

        private async Task OnYearChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int year))
            {
                selectedYear = year;
                txPage = 1;
                await Initialize();
            }
        }

        private void OnTxPageChange(int page)
        {
            txPage = page;
        }
    }
}
