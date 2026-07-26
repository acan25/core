namespace NArchitecture.Core.Security.Passkey;

public class PasskeyOptions
{
    /// <summary>
    /// The Relying Party ID — must be the registrable domain (e.g. "example.com"), or "localhost" for local dev.
    /// </summary>
    public string RelyingPartyId { get; set; } = string.Empty;
    public string RelyingPartyName { get; set; } = string.Empty;
    public string[] Origins { get; set; } = Array.Empty<string>();
    public int TimeoutMs { get; set; } = 60000;
}
