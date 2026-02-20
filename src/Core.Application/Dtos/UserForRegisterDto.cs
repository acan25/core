namespace NArchitecture.Core.Application.Dtos;

public class UserForRegisterDto : IDto
{
    public required string Email { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Role { get; set; }

    public UserForRegisterDto()
    {
        Email = string.Empty;
        Password = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Role = string.Empty;
    }

    public UserForRegisterDto(string email, string password, string firstName = "", string lastName = "", string role = "")
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }
}
