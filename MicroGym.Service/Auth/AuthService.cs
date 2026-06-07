using MicroGym.Data.Repository.UserRepository;

namespace MicroGym.Service.Auth
{
    public partial class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtService jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
        }
    }
}
