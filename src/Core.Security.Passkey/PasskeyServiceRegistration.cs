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
                Origins = new HashSet<string>(passkeyOptions.Origins),
                Timeout = (uint)passkeyOptions.TimeoutMs
            }
        ));
        services.AddScoped<IPasskeyService, Fido2PasskeyService>();

        return services;
    }
}
