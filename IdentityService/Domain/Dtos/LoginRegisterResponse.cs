namespace IdentityService.Domain.Dtos
{
    public class LoginRegisterResponse
    {
        public LoginRegisterResponse() { }
        public User? User { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }
    }
}
