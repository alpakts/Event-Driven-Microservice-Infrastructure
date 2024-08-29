using IdentityService.Application.Helpers;
using IdentityService.Application.Repositories;
using IdentityService.Application.Services.Auth;
using IdentityService.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Infrastructures.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUsersClaimsRepository _userClaimRepository;
        private readonly IClaimRepository _claimRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUsersClaimsRepository userClaimRepository, IUserRepository userRepository, JwtHelper jwtHelper, IClaimRepository claimRepository)
        {
            _userClaimRepository =userClaimRepository;
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _claimRepository = claimRepository;
        }

        public async Task<LoginRegisterResponse> Login(LoginDto loginInfo)
        {
            var user = await _userRepository.GetAsync(s => s.Username == loginInfo.Username);
            if (user is null)
            {
                return new LoginRegisterResponse() { Error = "User not found with this credentials" };
            }
            bool ifPasswordTrue = HashingHelper.VerifyPasswordHash(loginInfo.Password,user.PasswordHash,user.PasswordSalt);
            if (!ifPasswordTrue) return new LoginRegisterResponse() { Error = "User not found with this credentials" };
            return new LoginRegisterResponse()
            {
                User = user,
                Token = _jwtHelper.GenerateToken(user.UserClaims.Select(s => new Claim(s.Claim.ClaimType, s.Claim.Name)), DateTime.Now.AddDays(2))
            };


        }

        public async Task<LoginRegisterResponse> Register(RegisterDto registerInfo)
        {
            var user = await _userRepository.GetAsync(s => s.Username == registerInfo.Username);
            if (user is not null) return new LoginRegisterResponse() { Error = "Usernmae taken" };

            HashingHelper.CreatePasswordHash(registerInfo.Password, passwordHash: out byte[] passwordHash,passwordSalt: out byte[] passwordSalt);
            user = new User()
            {
                Email = registerInfo.Email,
                Username = registerInfo.Username,
                FullName = registerInfo.FullName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneCountryCode = registerInfo.PhoneCountryCode,
                PhoneNumber = registerInfo.PhoneNumber,
            };
            await _userRepository.AddAsync(user);
            var DefaultClaim = await _claimRepository.GetAsync(s => s.Id == 1);
            var defaulUserClaim = new List<IdentityService.Domain.Entities.UserClaim>() { new() { Claim = DefaultClaim, UserId = user.Id,ClaimId= DefaultClaim.ClaimId,User=user,Value=DefaultClaim.Name } };
            user.UserClaims = defaulUserClaim;
            await _userRepository.UpdateAsync(user);
            
            return new LoginRegisterResponse() { User=user,Token= _jwtHelper.GenerateToken(user.UserClaims.Select(s=>new Claim(s.Claim.ClaimType,s.Claim.Name)),DateTime.Now.AddDays(2))};
            
        }
    }
}
