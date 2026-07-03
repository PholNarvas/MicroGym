using Dapper;
using MicroGym.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MicroGym.Data.Repository.RevenueRepository
{
    public class RevenueRepository : IRevenueRepository
    {
        private readonly string connectionString;

        public RevenueRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection string is not configured in appsettings.");
        }

        public async Task<List<RevenuePaymentDetail>> GetRevenue(DateOnly month)
        {
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QueryAsync<RevenuePaymentDetail>(
                "pr_GetRevenues",
                new { Months = month.ToDateTime(TimeOnly.MinValue) },
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<Revenue?> GetYearlyRevenue()
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Revenue>(
                "pr_GetYearlyRevenue",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<RevenueChartMonth>> GetRevenueChartByYear(int year)
        {
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QueryAsync<RevenueChartMonth>(
                "pr_GetRevenueChartByYear",
                new { Year = year },
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
    }
}
