using IdentityService.Domain.Entities;
using IdentityService.Domain.Entities.@base;

public class User : Entity
{
    public User()
    {
    }

    public User(int ıd, string fullName, string username, byte[] passwordHash, byte[] PasswordSalt, string email, string phoneNumber, string phoneCountryCode, ICollection<UserClaim>? userClaims)
    {
        Id = ıd;
        FullName = fullName;
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        PhoneNumber = phoneNumber;
        PhoneCountryCode = phoneCountryCode;
        UserClaims = userClaims;
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneCountryCode { get; set; }
    public ICollection<UserClaim> UserClaims { get; set; }
}
