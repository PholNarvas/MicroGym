using Dapper;
using MicroGym.Shared.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MicroGym.Data.Repository.AttendanceRepository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly string connectionString;

        public AttendanceRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<AttendanceModel>> GetTodayAttendanceAsync()
        {
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QueryAsync<AttendanceModel>(
                "sp_GetTodayAttendance",
                commandType: System.Data.CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<bool> CheckInMemberAsync(int userId)
        {
            using var connection = new SqlConnection(connectionString);

            // The service layer already validated expiry and duplicate before reaching here.
            // SET NOCOUNT ON in the SP causes Dapper to return -1 regardless of rows inserted,
            // so we trust the service-level guards and return true after execution.
            await connection.ExecuteAsync(
                "pr_CheckInMember",
                new { UserID = userId },
                commandType: System.Data.CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> IsAlreadyCheckedInAsync(int userId)
        {
            using var connection = new SqlConnection(connectionString);

            var count = await connection.ExecuteScalarAsync<int>(
                "sp_IsAlreadyCheckedIn",
                new { UserID = userId },
                commandType: System.Data.CommandType.StoredProcedure);

            return count > 0;
        }

        public async Task<bool> IsMembershipValidAsync(int userId)
        {
            using var connection = new SqlConnection(connectionString);

            // Check if member's ExpiryDate is today or in the future.
            // If ExpiryDate is NULL or already passed, membership is invalid.
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM [dbo].[Users] WHERE [UserID] = @UserID AND CAST([ExpiryDate] AS DATE) >= CAST(GETDATE() AS DATE)",
                new { UserID = userId });

            return count > 0;
        }
    }
}
