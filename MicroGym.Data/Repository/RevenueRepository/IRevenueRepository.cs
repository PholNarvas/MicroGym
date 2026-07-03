using MicroGym.Shared.Model;

namespace MicroGym.Data.Repository.RevenueRepository
{
    public interface IRevenueRepository
    {
        Task<List<Payment>> GetRevenue(DateOnly month);
        Task<Revenue?> GetYearlyRevenue();
    }
}
