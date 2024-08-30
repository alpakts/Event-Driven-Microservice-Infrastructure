using IdentityService.Domain.Dtos;
using IdentityService.Domain.Entities;
using System.Linq.Expressions;

namespace IdentityService.Application.Services.Claim;
public interface IClaimService
{
    Task<ClaimResponseDto?> AddClaimAsync(Domain.Entities.Claim claim);
    Task<ClaimResponseDto> RemoveClaimAsync(Domain.Entities.Claim claim);
    Task<Domain.Entities.Claim> GetClaimAsync(Expression<Func<Domain.Entities.Claim, bool>> predicate);
    Task<IList<Domain.Entities.Claim>> GetClaimsAsync();

    Task<ClaimResponseDto> AddClaimToUserAsync(int userId , int claimId );
    Task<ClaimResponseDto> RemoveClaimFromUserAsync(int userId, int claimId);

}
