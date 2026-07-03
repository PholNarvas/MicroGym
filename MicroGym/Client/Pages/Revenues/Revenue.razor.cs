namespace MicroGym.Client.Pages.Revenues
{
    public partial class Revenue
    {
        // ── Stats ──────────────────────────────────────────────
        private decimal totalEarnings  = 0;
        private decimal monthlyIncome  = 0;
        private decimal monthlyExpense = 0;

        // ── Filter ─────────────────────────────────────────────
        private int selectedMonth;
        private int selectedYear;
        private List<int> availableYears = new();

        private string selectedMonthLabel =>
            new DateTime(selectedYear, selectedMonth, 1).ToString("MMMM");

        // ── UI State ───────────────────────────────────────────
        private bool isLoading   = true;
        private bool chartLoading = true;

        // ── Chart Data ─────────────────────────────────────────
        private double[] chartIncomeData  = new double[12];
        private double[] chartExpenseData = new double[12];
        private bool chartDataReady = false;
    }
}
