using MicroGym.Shared.Model;

namespace MicroGym.Data.Repository.RevenueRepository
{
    public interface IRevenueRepository
    {
        Task<List<RevenuePaymentDetail>> GetRevenue(DateOnly month);
        Task<Revenue?>                GetYearlyRevenue();
        Task<List<RevenueChartMonth>> GetRevenueChartByYear(int year);
    }
}
