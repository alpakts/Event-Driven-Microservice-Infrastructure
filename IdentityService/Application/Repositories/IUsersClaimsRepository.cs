using IdentityService.Application.Repositories.Base;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Repositories
{
    public interface IUsersClaimsRepository:IRepository<UserClaim>
    {
    }
}
