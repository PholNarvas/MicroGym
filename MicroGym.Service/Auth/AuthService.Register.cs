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

            var (success, _) = await userRepository.RegisterAsync(user);
            return success;
        }
    }
}
