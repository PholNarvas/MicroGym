using Dapper;
using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MicroGym.Data.Repository.MemberRepo
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string connectionString;

        public MemberRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<User>> GetAllMembers()
        {
            using var connection = new SqlConnection(connectionString);

            var userList = await connection.QueryAsync<User>("pr_GetAllUsers",
                commandType: System.Data.CommandType.StoredProcedure);

            return userList.ToList();
        }

        public async Task<User?> GetMemberByID(int memberid)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<User>("pr_GetUserByID",
                new { UserID = memberid }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<bool> SaveMemberInfo(EditMemberDto member)
        {
            using var connection = new SqlConnection(connectionString);

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserID", member.UserId, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@FirstName", member.FirstName, DbType.String);
                parameters.Add("@LastName", member.LastName, DbType.String);
                parameters.Add("@Email", member.Email, DbType.String);
                parameters.Add("@Phone", member.Phone, DbType.String);
                parameters.Add("@PaymentMethod", member.PaymentMethod, DbType.String);
                parameters.Add("@PaymentAmount", member.Price, DbType.Decimal);
                parameters.Add("@MemberShipTypeID", member.MemberShipTypeID, DbType.Int32);
                parameters.Add("@IsActive", member.IsActive, DbType.Boolean);

                await connection.ExecuteAsync(
                    "pr_SaveMemberInfo",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var returnedUserID = parameters.Get<int>("@UserID");
                return returnedUserID > 0;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<bool> DeleteMember(int userID)
        {
            using var connection = new SqlConnection(connectionString);

            var rowsAffected = await connection.ExecuteAsync(
                "pr_DeleteMember",
                new { UserID = userID },
                commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }
    }
}
