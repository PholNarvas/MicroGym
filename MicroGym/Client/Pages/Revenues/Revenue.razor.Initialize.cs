namespace MicroGym.Client.Pages.Revenues
{
    public partial class Revenue
    {
        protected override async Task OnInitializedAsync()
        {
            selectedMonth = DateTime.Now.Month;
            selectedYear  = DateTime.Now.Year;

            availableYears = Enumerable.Range(DateTime.Now.Year - 3, 4)
                                       .OrderByDescending(y => y)
                                       .ToList();

            await Initialize();
        }

        private async Task Initialize()
        {
            isLoading    = true;
            chartLoading = true;
            StateHasChanged();

            // ── Load stat cards first (fast) ───────────────────
            var payments = await RevenueService.GetRevenue(selectedMonth, selectedYear);
            var revenue  = await GetYearlyRevenue();

            totalEarnings  = revenue?.TotalRevenue ?? 0;
            monthlyIncome  = payments.Where(p => p.Status == "Paid").Sum(p => p.AmountPaid);
            monthlyExpense = payments.Where(p => p.Status == "Expense").Sum(p => p.AmountPaid);

            isLoading = false;
            StateHasChanged(); // show stat cards

            // ── Load chart data (12 months) ────────────────────
            await LoadChartData();
            StateHasChanged(); // triggers OnAfterRenderAsync → renders chart
        }
    }
}
