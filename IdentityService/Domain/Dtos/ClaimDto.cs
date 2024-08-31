using IdentityService.Domain.Abstraction;
//todo validations can be added with fluent validation lib
namespace IdentityService.Domain.Dtos;
public class ClaimDto : IDto
{
    public int ClaimId { get; set; }
    public string ClaimType { get; set; }
    public string Name { get; set; }


    public ClaimDto(int claimId, string claimType, string name)
    {
        ClaimId = claimId;
        ClaimType = claimType;
        Name = name;
    }

    public ClaimDto()
    {
    }
}
