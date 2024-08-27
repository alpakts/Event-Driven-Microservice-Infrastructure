using IdentityService.Domain.Abstraction;

namespace IdentityService.Domain.Dtos
{
    public class RegisterDto : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCountryCode { get; set; }
        public string FullName { get; set; }

    }
    public class LoginDto : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
