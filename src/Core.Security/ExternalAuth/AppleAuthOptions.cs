namespace NArchitecture.Core.Security.ExternalAuth;

public class AppleAuthOptions
{
    /// <summary>
    /// The Services ID registered in Apple Developer for "Sign in with Apple" (used as the JWT `aud` claim).
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
    public string Issuer { get; set; } = "https://appleid.apple.com";
    public string JwksUri { get; set; } = "https://appleid.apple.com/auth/keys";
}
