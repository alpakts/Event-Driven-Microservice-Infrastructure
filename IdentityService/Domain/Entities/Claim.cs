using IdentityService.Domain.Entities.@base;

namespace IdentityService.Domain.Entities;
public class Claim : Entity
{
    public int ClaimId { get; set; }
    public string ClaimType { get; set; }
    public string Name { get; set; }

    public ICollection<UserClaim> UserClaims { get; set; }

    public Claim(int claimId, string claimType, ICollection<UserClaim> userClaims,string name )
    {
        ClaimId = claimId;
        ClaimType = claimType;
        UserClaims = userClaims;
        Name = name;
    }

    public Claim()
    {
    }
}
