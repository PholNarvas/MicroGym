using MicroGym.Shared.DTOs;
using MicroGym.Shared.Model;

namespace MicroGym.Service.Auth
{
    public partial class AuthService
    {
        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                MemberShipTypeID = request.MemberShipTypeID,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Member",
                CreatedAt = DateTime.UtcNow,
                PaymentMethod = request.PaymentMethod,
                Price = request.PaymentAmount
            };

            var (success, newUserId) = await userRepository.RegisterAsync(user);

            if (!success) return false;

            if (request.TierID.HasValue && newUserId > 0)
            {
                await memberRepository.PurchaseAnnualMembership(new PurchaseAnnualMembershipDto
                {
                    UserId        = newUserId,
                    TierID        = request.TierID.Value,
                    PaymentMethod = request.PaymentMethod
                });
            }

            return true;
        }
    }
}
