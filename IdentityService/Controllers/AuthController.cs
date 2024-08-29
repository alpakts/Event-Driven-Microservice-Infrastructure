using IdentityService.Application.Helpers;
using IdentityService.Application.Services.Auth;
using IdentityService.Domain.Dtos;
using IdentityService.Infrastructures.Services.Queue.Kafka;
using IdentityService.Infrastructures.Services.Queue.Kafka.Events;
using IdentityService.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly KafkaProducer _producer;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(AuthDbContext context, KafkaProducer producer, IConfiguration configuration, IAuthService authService)
        {
            _context = context;
            _producer = producer;
            _configuration = configuration;
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var response = await _authService.Register(request);
            if(response.Error != null) return Ok(response);
            return BadRequest(response);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var response =  await _authService.Login(request);
            if (response.Error != null) return Ok(response);
            return BadRequest(response);


        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Username),
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }


}
