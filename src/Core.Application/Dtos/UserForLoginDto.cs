namespace NArchitecture.Core.Application.Dtos;

public class UserForLoginDto : IDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? AuthenticatorCode { get; set; }

    public UserForLoginDto()
    {
        Email = string.Empty;
        Password = string.Empty;
        AuthenticatorCode = null;
    }

    public UserForLoginDto(string email, string password, string? authenticatorCode = null)
    {
        Email = email;
        Password = password;
        AuthenticatorCode = authenticatorCode;
    }
}
