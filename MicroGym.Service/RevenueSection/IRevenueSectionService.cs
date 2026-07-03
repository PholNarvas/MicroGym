using MicroGym.Shared.Model;

namespace MicroGym.Service.RevenueSection
{
    public interface IRevenueSectionService
    {
        Task<List<Payment>> GetRevenueService(DateOnly month);
        Task<Revenue?> GetRevenueService();
    }
}
