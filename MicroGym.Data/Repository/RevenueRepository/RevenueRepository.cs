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
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Payment>> GetRevenue(DateOnly month)
        {
            using var connection = new SqlConnection(connectionString);

            var parameters = new
            {
                Months = month.ToDateTime(TimeOnly.MinValue)
            };

            var result = await connection.QueryAsync<Payment>(
                "pr_GetRevenues",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<Revenue?> GetYearlyRevenue()
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Revenue>(
            "pr_GetYearlyRevenue",
            commandType: CommandType.StoredProcedure);

        }
    }
}
