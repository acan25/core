namespace NArchitecture.Core.Security.Passkey;

public class PasskeyOptions
{
    /// <summary>
    /// The Relying Party ID — must be the registrable domain (e.g. "example.com"), or "localhost" for local dev.
    /// </summary>
    public string RelyingPartyId { get; set; } = string.Empty;
    public string RelyingPartyName { get; set; } = string.Empty;
    public string[] Origins { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Android uygulamasının imza sertifikasının SHA-256 parmak izleri ("AB:CD:..." — assetlinks.json'daki
    /// değerin aynısı). Android'de passkey seremonisinin <c>clientDataJSON.origin</c> alanı web adresi değil
    /// <c>android:apk-key-hash:&lt;base64url(sha256)&gt;</c> olur; bu liste o kaynakları otomatik üretip
    /// <see cref="Origins"/>'e ekler. Boşsa yalnızca web kaynakları geçerlidir ve mobil Android passkey girişi reddedilir.
    /// </summary>
    public string[] AndroidCertificateFingerprints { get; set; } = Array.Empty<string>();
    public int TimeoutMs { get; set; } = 60000;
}
