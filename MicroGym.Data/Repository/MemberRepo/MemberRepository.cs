using Dapper;
using MicroGym.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MicroGym.Data.Repository.MemberRepo
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string connectionString;

        public MemberRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Members>> GetAllMembers()
        {
            using var connection = new SqlConnection(connectionString);

            var memberList = await connection.QueryAsync<Members>("pr_GetActiveMember",
                commandType: System.Data.CommandType.StoredProcedure);

            return memberList.ToList();

        }

    }
}
