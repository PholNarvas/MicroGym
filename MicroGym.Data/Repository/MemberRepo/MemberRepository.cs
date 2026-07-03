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
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection string is not configured in appsettings.");
        }

        public async Task<List<User>> GetAllMembers()
        {
            using var connection = new SqlConnection(connectionString);

            var userList = await connection.QueryAsync<User>("pr_GetAllUsers",
                commandType: System.Data.CommandType.StoredProcedure);

            return userList.ToList();
        }

        public async Task<List<User>> GetExpiringMembers()
        {
            using var connection = new SqlConnection(connectionString);

            var userList = await connection.QueryAsync<User>("pr_GetMembersExpiring7Days",
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

            var result = await connection.ExecuteScalarAsync<int>(
                "pr_SaveMemberInfo",
                new
                {
                    UserID           = member.UserId,
                    FirstName        = member.FirstName,
                    LastName         = member.LastName,
                    Email            = member.Email,
                    Phone            = member.Phone,
                    MemberShipTypeID = member.MemberShipTypeID,
                    IsActive         = member.IsActive
                },
                commandType: CommandType.StoredProcedure);

            return result switch
            {
                1  => true,
                -1 => throw new InvalidOperationException("Email is already in use by another member."),
                _  => false  // 0 = user not found
            };
        }

        public async Task<bool> RenewMember(RenewMemberDto dto)
        {
            using var connection = new SqlConnection(connectionString);

            var result = await connection.ExecuteScalarAsync<int>(
                "pr_RenewMember",
                new
                {
                    UserID           = dto.UserId,
                    MemberShipTypeID = dto.MemberShipTypeID,
                    PaymentAmount    = dto.PaymentAmount,
                    PaymentMethod    = dto.PaymentMethod
                },
                commandType: CommandType.StoredProcedure);

            return result switch
            {
                1  => true,
                0  => throw new InvalidOperationException("Member not found."),
                -1 => throw new InvalidOperationException("Membership type not found."),
                _  => throw new InvalidOperationException($"Unexpected result from database: {result}")
            };
        }

        public async Task<bool> PurchaseAnnualMembership(PurchaseAnnualMembershipDto dto)
        {
            using var connection = new SqlConnection(connectionString);

            var result = await connection.ExecuteScalarAsync<int>(
                "pr_PurchaseAnnualMembership",
                new
                {
                    UserID = dto.UserId,
                    TierID = dto.TierID,
                    PaymentMethod = dto.PaymentMethod
                },
                commandType: CommandType.StoredProcedure);

            return result switch
            {
                1 => true,
                0 => throw new InvalidOperationException("Member not found."),
                -1 => throw new InvalidOperationException("Membership tier not found or is no longer active."),
                _ => throw new InvalidOperationException($"Unexpected result from database: {result}")
            };
        }

        public async Task<List<MembershipTier>> GetMembershipTiers()
        {
            using var connection = new SqlConnection(connectionString);

            var tiers = await connection.QueryAsync<MembershipTier>(
                "pr_GetMembershipTiers",
                commandType: CommandType.StoredProcedure);

            return tiers.ToList();
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

        public async Task<List<MemberPaymentRecord>> GetMemberPaymentHistory(int userId)
        {
            using var connection = new SqlConnection(connectionString);

            var records = await connection.QueryAsync<MemberPaymentRecord>(
                "pr_GetMemberPaymentHistory",
                new { UserID = userId },
                commandType: CommandType.StoredProcedure);

            return records.ToList();
        }

        public async Task<bool> UpdateProfilePicture(UpdateProfilePictureDto dto)
        {
            using var connection = new SqlConnection(connectionString);

            var rowsAffected = await connection.ExecuteScalarAsync<int>(
                "pr_UpdateProfilePicture",
                new { UserID = dto.UserId, ProfilePicture = dto.ProfilePicture },
                commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }

        public async Task<int> AddExistingMember(AddExistingMemberDto dto, string passwordHash)
        {
            using var connection = new SqlConnection(connectionString);

            var result = await connection.QueryFirstOrDefaultAsync<int>(
                "pr_AddExistingMember",
                new
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    MemberShipTypeID = dto.MemberShipTypeID,
                    PasswordHash = passwordHash,
                    DateJoined = dto.DateJoined,
                    ExpiryDate = dto.ExpiryDate,
                    TierID = dto.TierID,
                    TierExpiryDate = dto.TierExpiryDate,
                    PaymentAmount = dto.PaymentAmount ?? 0,
                    PaymentMethod = dto.PaymentMethod
                },
                commandType: CommandType.StoredProcedure);

            return result; // -1 = duplicate email, > 0 = new UserID
        }
    }
}
