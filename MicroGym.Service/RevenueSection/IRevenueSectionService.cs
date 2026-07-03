using MicroGym.Shared.Model;

namespace MicroGym.Service.RevenueSection
{
    public interface IRevenueSectionService
    {
        Task<List<RevenuePaymentDetail>> GetRevenueService(DateOnly month);
        Task<Revenue?>                GetRevenueService();
        Task<List<RevenueChartMonth>> GetRevenueChartByYearService(int year);
    }
}
