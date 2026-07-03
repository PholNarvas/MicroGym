using MicroGym.Data.Repository.MemberRepo;
using MicroGym.Data.Repository.UserRepository;

namespace MicroGym.Service.Auth
{
    public partial class AuthService : IAuthService
    {
        private readonly IUserRepository   userRepository;
        private readonly IMemberRepository memberRepository;
        private readonly IJwtService       jwtService;

        public AuthService(
            IUserRepository   userRepository,
            IMemberRepository memberRepository,
            IJwtService       jwtService)
        {
            this.userRepository   = userRepository;
            this.memberRepository = memberRepository;
            this.jwtService       = jwtService;
        }
    }
}
