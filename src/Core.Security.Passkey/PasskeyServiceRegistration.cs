using Fido2NetLib;
using Microsoft.Extensions.DependencyInjection;

namespace NArchitecture.Core.Security.Passkey;

public static class PasskeyServiceRegistration
{
    public static IServiceCollection AddPasskeyServices(this IServiceCollection services, PasskeyOptions passkeyOptions)
    {
        services.AddSingleton(passkeyOptions);
        services.AddSingleton<IFido2>(_ => new Fido2(
            new Fido2Configuration
            {
                ServerDomain = passkeyOptions.RelyingPartyId,
                ServerName = passkeyOptions.RelyingPartyName,
                Origins = new HashSet<string>(passkeyOptions.Origins.Concat(androidOrigins(passkeyOptions))),
                Timeout = (uint)passkeyOptions.TimeoutMs
            }
        ));
        services.AddScoped<IPasskeyService, Fido2PasskeyService>();

        return services;
    }

    private static IEnumerable<string> androidOrigins(PasskeyOptions options) =>
        options.AndroidCertificateFingerprints
            .Where(fingerprint => !string.IsNullOrWhiteSpace(fingerprint))
            .Select(fingerprint =>
            {
                byte[] hash = Convert.FromHexString(fingerprint.Replace(":", string.Empty).Trim());
                string base64Url = Convert.ToBase64String(hash).TrimEnd('=').Replace('+', '-').Replace('/', '_');
                return $"android:apk-key-hash:{base64Url}";
            });
}
