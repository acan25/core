namespace NArchitecture.Core.Security.ExternalAuth;

public interface IAppleAuthVerifier
{
    /// <exception cref="ExternalAuthVerificationException">Thrown when the id token is invalid.</exception>
    Task<ExternalAuthPrincipal> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default);
}
