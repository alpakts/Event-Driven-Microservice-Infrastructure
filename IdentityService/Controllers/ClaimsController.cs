using IdentityService.Application.Services.Claim;
using IdentityService.Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//todo can be added user service for mananing users
namespace IdentityService.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet("{claimId}")]
        public async Task<IActionResult> GetClaim([FromRoute] int claimId)
        {
            var result =await _claimService.GetClaimAsync(s => s.Id == claimId);
            if (result != null)return Ok(result);
            return BadRequest(result);
        }
        [HttpGet()]
        public async Task<IActionResult> GetClaims()
        {
            var result = await _claimService.GetClaimsAsync();
            return Ok(result);
        }
        [HttpPost()]
        public async Task<IActionResult> AddClaim([FromBody] ClaimDto request)
        {
            var claimToCreate = new Domain.Entities.Claim()
            {
                ClaimId = request.ClaimId,
                ClaimType = request.ClaimType,  
                Name = request.Name,
            };
            var result = await _claimService.AddClaimAsync(claimToCreate);
            if (result != null) return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddClaimToUser([FromQuery] int userId,[FromQuery] int claimId)
        {
            var result = await _claimService.AddClaimToUserAsync(userId, claimId);
            if (result != null) return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveClaimFromUser([FromQuery] int userId, [FromQuery] int claimId)
        {
            var result = await _claimService.RemoveClaimFromUserAsync(userId, claimId);
            if (result != null) return Ok(result);
            return BadRequest(result);
        }

    }
}
