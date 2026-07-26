namespace NArchitecture.Core.Security.ExternalAuth;

/// <summary>
/// Thrown when an external provider's identity token (Google/Apple id_token) fails verification
/// (invalid signature, expired, wrong audience/issuer, etc.).
/// </summary>
public class ExternalAuthVerificationException : Exception
{
    public ExternalAuthVerificationException(string message)
        : base(message) { }

    public ExternalAuthVerificationException(string message, Exception innerException)
        : base(message, innerException) { }
}
