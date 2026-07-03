using MicroGym.Data.Repository.RevenueRepository;
using MicroGym.Shared.Model;

namespace MicroGym.Service.RevenueSection
{
    public class RevenueSectionService : IRevenueSectionService
    {
        private readonly IRevenueRepository _revenueRepository;

        public RevenueSectionService(IRevenueRepository revenueRepository)
        {
            _revenueRepository = revenueRepository;
        }

        public async Task<List<RevenuePaymentDetail>> GetRevenueService(DateOnly month)
        {
            return await _revenueRepository.GetRevenue(month);
        }

        public async Task<Revenue?> GetRevenueService()
        {
            return await _revenueRepository.GetYearlyRevenue();
        }

        public async Task<List<RevenueChartMonth>> GetRevenueChartByYearService(int year)
        {
            return await _revenueRepository.GetRevenueChartByYear(year);
        }
    }
}
