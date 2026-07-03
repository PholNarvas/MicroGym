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
            try
            {
                isLoading    = true;
                chartLoading = true;
                StateHasChanged();

                // Previous month (handles Jan → Dec of prior year)
                var prevMonth = selectedMonth == 1 ? 12 : selectedMonth - 1;
                var prevYear  = selectedMonth == 1 ? selectedYear - 1 : selectedYear;

                // Fetch current month, yearly total, and previous month in parallel
                var currentTask  = RevenueService.GetRevenue(selectedMonth, selectedYear);
                var yearlyTask   = GetYearlyRevenue();
                var previousTask = RevenueService.GetRevenue(prevMonth, prevYear);

                await Task.WhenAll(currentTask, yearlyTask, previousTask);

                var payments     = currentTask.Result;
                var revenue      = yearlyTask.Result;
                var prevPayments = previousTask.Result;

                // KPI cards
                paymentDetails      = payments;
                totalEarnings       = revenue?.TotalRevenue ?? 0;
                monthlyIncome       = payments.Where(p => p.Status == "Paid").Sum(p => p.AmountPaid);
                monthlyExpense      = payments.Where(p => p.Status == "Expense").Sum(p => p.AmountPaid);
                monthlyNet          = monthlyIncome - monthlyExpense;
                previousMonthIncome = prevPayments.Where(p => p.Status == "Paid").Sum(p => p.AmountPaid);

                // Plan vs Tier breakdown
                planIncome = payments.Where(p => p.Status == "Paid" && p.PaymentType == "Plan").Sum(p => p.AmountPaid);
                tierIncome = payments.Where(p => p.Status == "Paid" && p.PaymentType == "Tier").Sum(p => p.AmountPaid);

                isLoading = false;
                StateHasChanged();

                await LoadChartData();
                StateHasChanged();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception)
            {
                // Network failure or bad JSON — stat cards stay at zero.
            }
            finally
            {
                isLoading    = false;
                chartLoading = false;
            }
        }
    }
}
