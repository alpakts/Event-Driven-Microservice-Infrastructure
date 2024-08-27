using IdentityService.Domain.Entities.@base;

namespace IdentityService.Domain.Entities;
public class UserClaim : Entity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int ClaimId { get; set; }
    public Claim Claim { get; set; }

    public string? Value { get; set; }

    public UserClaim(int userId, User user, int claimId, Claim claim, string? value)
    {
        UserId = userId;
        User = user;
        ClaimId = claimId;
        Claim = claim;
        Value = value;
    }

    public UserClaim()
    {
    }
}
