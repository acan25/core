using Google.Apis.Auth;

namespace NArchitecture.Core.Security.ExternalAuth;

public class GoogleAuthVerifier : IGoogleAuthVerifier
{
    private readonly GoogleAuthOptions _options;

    public GoogleAuthVerifier(GoogleAuthOptions options)
    {
        _options = options;
    }

    public async Task<ExternalAuthPrincipal> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
    {
        try
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings { Audience = [_options.ClientId] }
            );

            return new ExternalAuthPrincipal(
                ProviderUserId: payload.Subject,
                Email: payload.Email,
                EmailVerified: payload.EmailVerified,
                FirstName: payload.GivenName,
                LastName: payload.FamilyName
            );
        }
        catch (InvalidJwtException ex)
        {
            throw new ExternalAuthVerificationException("Google id_token doğrulanamadı.", ex);
        }
    }
}
