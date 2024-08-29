using IdentityService.Domain.Dtos;

namespace IdentityService.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginRegisterResponse> Login(LoginDto loginInfo);
        Task<LoginRegisterResponse> Register(RegisterDto registerInfo );
    }
}
