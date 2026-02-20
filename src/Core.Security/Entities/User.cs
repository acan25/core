using NArchitecture.Core.Persistence.Repositories;
using NArchitecture.Core.Security.Enums;

namespace NArchitecture.Core.Security.Entities;

public class User<TId> : Entity<TId>
{
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public User()
    {
        Email = string.Empty;
        PasswordHash = Array.Empty<byte>();
        PasswordSalt = Array.Empty<byte>();
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public User(string email, byte[] passwordSalt, byte[] passwordHash, AuthenticatorType authenticatorType, string firstName = "", string lastName = "")
    {
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        AuthenticatorType = authenticatorType;
        FirstName = firstName;
        LastName = lastName;
    }

    public User(TId id, string email, byte[] passwordSalt, byte[] passwordHash, AuthenticatorType authenticatorType, string firstName = "", string lastName = "")
        : base(id)
    {
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        AuthenticatorType = authenticatorType;
        FirstName = firstName;
        LastName = lastName;
    }
}
