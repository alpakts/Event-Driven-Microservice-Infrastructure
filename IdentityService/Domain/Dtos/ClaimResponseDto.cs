using IdentityService.Domain.Abstraction;
using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Dtos
{
    public class ClaimResponseDto:IDto
    {
        public string? ClaimType { get; set; }
        public int ClaimId { get; set; }
        public string? ClaimName { get; set; }

        public List<UserClaim>? UserClaims { get; set; }
    }
}
