using IdentityService.Application.Repositories;
using IdentityService.Persistence.Context;
using IdentityService.Persistence.Repositories.Base;

namespace IdentityService.Persistence.Repositories
{
    public class UserRepository : EfRepositoryBase<AuthDbContext, User>, IUserRepository
    {
        public UserRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
