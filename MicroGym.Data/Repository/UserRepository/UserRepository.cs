using Dapper;
using MicroGym.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MicroGym.Data.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(connectionString);
            const string sql = "SELECT * FROM Users WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<(bool Success, int NewUserId)> RegisterAsync(User user)
        {
            using var connection = new SqlConnection(connectionString);

            var parameters = new
            {
                user.MemberShipTypeID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Phone,
                user.PasswordHash,
                user.Role,
                user.CreatedAt,
                user.PaymentMethod,
                PaymentAmount = user.Price
            };

            var result = await connection.QueryFirstOrDefaultAsync(
                "pr_RegisterUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null || result.StatusCode != "SUCCESS")
                return (false, 0);

            return (true, (int)result.NewUserId);
        }
    }
}
