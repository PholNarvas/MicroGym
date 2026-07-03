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

        /// <summary>
        /// Fetches income/expense for all 12 months in a single SP call
        /// instead of 12 separate requests.
        /// </summary>
        private async Task LoadChartData()
        {
            chartLoading = true;
            chartDataReady = false;

            // Reset arrays so months with no data show as zero
            chartIncomeData = new double[12];
            chartExpenseData = new double[12];

            var rows = await RevenueService.GetRevenueChartByYearAsync(selectedYear);

            foreach (var row in rows)
            {
                var index = row.Month - 1; // Month is 1-based, array is 0-based
                chartIncomeData[index] = (double)row.Income;
                chartExpenseData[index] = (double)row.Expense;
            }

            chartLoading = false;
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
        private IEnumerable<(string Method, decimal Amount)> paymentMethodBreakdown =>
      paymentDetails
          .Where(p => p.Status == "Paid")
          .GroupBy(p => p.PaymentMethod)
          .Select(g => (Method: g.Key, Amount: g.Sum(p => p.AmountPaid)))
          .OrderByDescending(x => x.Amount);

        // ?? Computed: Month-over-month ?????????????????????????
        private string MoMText
        {
            get
            {
                if (previousMonthIncome == 0)
                    return monthlyIncome > 0 ? "New data this month" : "No data";

                var diff = monthlyIncome - previousMonthIncome;
                var pct = Math.Abs(diff) / previousMonthIncome * 100;
                return diff >= 0
                    ? $"? {pct:N0}% vs last month"
                    : $"? {pct:N0}% vs last month";
            }
        }

        private string MoMClass => previousMonthIncome == 0
            ? "mom-neutral"
            : monthlyIncome >= previousMonthIncome ? "mom-up" : "mom-down";
    }
}
