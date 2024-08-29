using IdentityService.Application.Repositories;
using IdentityService.Domain.Entities;
using IdentityService.Persistence.Context;
using IdentityService.Persistence.Repositories.Base;

namespace IdentityService.Persistence.Repositories
{
    public class UsersClaimsRepository : EfRepositoryBase<AuthDbContext, UserClaim>, IUsersClaimsRepository
    {
        public UsersClaimsRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
