using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MicroGym.Client.Pages.Revenues
{
    public partial class Revenue
    {
        [Inject] private IJSRuntime JS { get; set; } = default!;

        private static readonly string[] MonthLabels =
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        /// <summary>Fetches income/expense for every month in the selected year in parallel.</summary>
        private async Task LoadChartData()
        {
            chartLoading   = true;
            chartDataReady = false;

            var tasks = Enumerable.Range(1, 12)
                .Select(m => RevenueService.GetRevenue(m, selectedYear))
                .ToArray();

            var results = await Task.WhenAll(tasks);

            for (int i = 0; i < 12; i++)
            {
                chartIncomeData[i]  = (double)results[i].Where(p => p.Status == "Paid").Sum(p => p.AmountPaid);
                chartExpenseData[i] = (double)results[i].Where(p => p.Status == "Expense").Sum(p => p.AmountPaid);
            }

            chartLoading   = false;
            chartDataReady = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (chartDataReady)
            {
                chartDataReady = false;
                await JS.InvokeVoidAsync(
                    "microgym.initRevenueChart",
                    "revenue-chart",
                    MonthLabels,
                    chartIncomeData,
                    chartExpenseData);
            }
        }
    }
}
