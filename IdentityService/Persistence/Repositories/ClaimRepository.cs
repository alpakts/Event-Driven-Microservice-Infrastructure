using IdentityService.Application.Repositories;
using IdentityService.Domain.Entities;
using IdentityService.Persistence.Context;
using IdentityService.Persistence.Repositories.Base;

namespace IdentityService.Persistence.Repositories
{
    public class ClaimRepository : EfRepositoryBase<AuthDbContext, Claim>, IClaimRepository
    {
        public ClaimRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
