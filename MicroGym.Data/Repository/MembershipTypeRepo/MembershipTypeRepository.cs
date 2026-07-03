using Dapper;
using MicroGym.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MicroGym.Data.Repository.MembershipTypeRepo
{
    public class MembershipTypeRepository : IMembershipTypeRepository
    {
        private readonly string connectionString;

        public MembershipTypeRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<MembershipType>> GetAllMembershipTypes()
        {
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QueryAsync<MembershipType>(
                "pr_GetMembershipTypes",
                commandType: System.Data.CommandType.StoredProcedure);

            return result.ToList();
        }
    }
}
