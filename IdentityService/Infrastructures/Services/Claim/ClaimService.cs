using IdentityService.Application.Repositories;
using IdentityService.Application.Services.Claim;
using IdentityService.Domain.Dtos;
using IdentityService.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace IdentityService.Infrastructures.Services.Claim;
public class ClaimService : IClaimService
{
    private readonly IClaimRepository _claimRepository;
    private readonly IUsersClaimsRepository _userClaimRepository;
    private readonly IUserRepository _userRepository;

    public ClaimService(IClaimRepository claimRepository, IUsersClaimsRepository userClaimRepository, IUserRepository userRepository)
    {
        _claimRepository = claimRepository;
        _userClaimRepository = userClaimRepository;
        _userRepository = userRepository;
    }

    public async Task<ClaimResponseDto?> AddClaimAsync(Domain.Entities.Claim claim)
    {
        var isClaimExist = await _claimRepository.AnyAsync(s => s.Id == claim.Id);
        if (!isClaimExist)
        {
            _claimRepository.AddAsync(claim);
            return new() { ClaimId = claim.ClaimId, ClaimName = claim.Name, ClaimType = claim.ClaimType };
        }
        return null;

    }

    public async Task<ClaimResponseDto> AddClaimToUserAsync(int userId, int claimId)
    {
        var user = await _userRepository.GetAsync(s => s.Id == userId);
        var claim = await _claimRepository.GetAsync(s=>s.Id == claimId);
        if (user == null|| claim == null) throw new Exception("user not found");
        UserClaim userClaim= new UserClaim(userId,user,claimId,claim,claim.Name);
        await  _userClaimRepository.AddAsync(userClaim);
        return new ClaimResponseDto()
        {
            ClaimId = claim.Id,
            ClaimName = claim.Name,
            ClaimType = claim.ClaimType,
            UserClaims = user.UserClaims.ToList()
        };


    }

    public async  Task<Domain.Entities.Claim> GetClaimAsync(Expression<Func<Domain.Entities.Claim, bool>> predicate)
    {
        return await _claimRepository.GetAsync(predicate);
    }

    public async Task<IList<Domain.Entities.Claim>> GetClaimsAsync()
    {
        return await _claimRepository.GetListAsync();
    }

    public async Task<ClaimResponseDto> RemoveClaimAsync(Domain.Entities.Claim claim)
    {
       await _claimRepository.DeleteAsync(claim);
        return new ClaimResponseDto()
        {

            ClaimId = claim.Id,
            ClaimName = claim.Name,
            ClaimType = claim.ClaimType,
            UserClaims = claim.UserClaims?.ToList()
        };
    }

    public async Task<ClaimResponseDto> RemoveClaimFromUserAsync(int userId, int claimId)
    {
        var user = await _userRepository.GetAsync(s=>s.Id ==  userId);
        var claim =await  _claimRepository.GetAsync(s=>s.Id == claimId);
        if (user is null || claim is null)
        {
            throw new ArgumentException("user or claim is null");
        }
        UserClaim userClaim = new()
        {
            ClaimId = claimId,
            Claim = claim,
            UserId = userId,
            User = user,
            Value = claim.Name
        };
        user.UserClaims.Add(userClaim);
        await _userClaimRepository.AddAsync(userClaim);
        return new ClaimResponseDto()
        {
            ClaimId = claimId,
            ClaimName = claim.Name,
            ClaimType = claim.ClaimType,
            UserClaims = claim.UserClaims?.ToList()
        };
    }
}
