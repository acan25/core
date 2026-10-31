namespace NArchitecture.Core.Security.ExternalAuth;

public class AppleAuthOptions
{
    /// <summary>
    /// The Services ID registered in Apple Developer for "Sign in with Apple" (used as the JWT `aud` claim).
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Ek geçerli <c>aud</c> değerleri. iOS uygulamasının yerel "Apple ile giriş" akışı token'ın hedefine Services ID'yi
    /// değil uygulamanın bundle kimliğini (<c>com.dogruteklif.app</c>) yazar; mobil girişin çalışması için buraya eklenir.
    /// </summary>
    public string[] AdditionalClientIds { get; set; } = Array.Empty<string>();

    public string Issuer { get; set; } = "https://appleid.apple.com";
    public string JwksUri { get; set; } = "https://appleid.apple.com/auth/keys";
}
