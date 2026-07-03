using MicroGym.Shared.Model;

namespace MicroGym.Client.Pages.Revenues
{
    public partial class Revenue
    {
        // ── Stats ──────────────────────────────────────────────
        private decimal totalEarnings = 0;
        private decimal monthlyIncome = 0;
        private decimal monthlyExpense = 0;
        private decimal monthlyNet = 0;
        private decimal planIncome = 0;
        private decimal tierIncome = 0;
        private decimal previousMonthIncome = 0;

        // ── Transactions ───────────────────────────────────────
        private List<RevenuePaymentDetail> paymentDetails = new();

        // ── Pagination ─────────────────────────────────────────
        private int txPage = 1;
        private const int TxPageSize = 30;

        private int txTotalPages => (int)Math.Ceiling(paymentDetails.Count / (double)TxPageSize);

        private IEnumerable<RevenuePaymentDetail> pagedTransactions =>
            paymentDetails.Skip((txPage - 1) * TxPageSize).Take(TxPageSize);

        // ── Filter ─────────────────────────────────────────────
        private int selectedMonth;
        private int selectedYear;
        private List<int> availableYears = new();

        private string selectedMonthLabel =>
            new DateTime(selectedYear, selectedMonth, 1).ToString("MMMM");

        // ── Revenue PIN Lock ───────────────────────────────────
        private bool isRevenueLocked = true;
        private string pinInput = string.Empty;
        private string pinError = string.Empty;
        private const string RevenuePIN = "GNG2008"; // Change this to your preferred PIN

        // ── UI State ───────────────────────────────────────────
        private bool isLoading = true;
        private bool chartLoading = true;

        // ── Chart Data ─────────────────────────────────────────
        private double[] chartIncomeData = new double[12];
        private double[] chartExpenseData = new double[12];
        private bool chartDataReady = false;

        // ── Computed: Payment method breakdown ─────────────────

    }
}
