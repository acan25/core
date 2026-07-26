namespace NArchitecture.Core.Security.ExternalAuth;

public record ExternalAuthPrincipal(string ProviderUserId, string Email, bool EmailVerified, string? FirstName, string? LastName);
