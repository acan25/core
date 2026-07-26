using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace NArchitecture.Core.Security.ExternalAuth;

public class AppleAuthVerifier : IAppleAuthVerifier
{
    private const string JwksCacheKey = "NArchitecture.Core.Security.ExternalAuth.AppleJwks";
    private static readonly TimeSpan JwksCacheDuration = TimeSpan.FromHours(6);

    private readonly AppleAuthOptions _options;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;

    public AppleAuthVerifier(AppleAuthOptions options, HttpClient httpClient, IMemoryCache memoryCache)
    {
        _options = options;
        _httpClient = httpClient;
        _memoryCache = memoryCache;
    }

    public async Task<ExternalAuthPrincipal> VerifyIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
    {
        JsonWebKeySet jwks = await getAppleJwksAsync(cancellationToken);

        JwtSecurityTokenHandler handler = new();
        ClaimsPrincipal claimsPrincipal;
        try
        {
            claimsPrincipal = handler.ValidateToken(
                idToken,
                new TokenValidationParameters
                {
                    ValidIssuer = _options.Issuer,
                    ValidAudience = _options.ClientId,
                    IssuerSigningKeys = jwks.Keys,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                },
                out _
            );
        }
        catch (Exception ex) when (ex is SecurityTokenException or ArgumentException)
        {
            throw new ExternalAuthVerificationException("Apple id_token doğrulanamadı.", ex);
        }

        string subject = claimsPrincipal.FindFirst("sub")?.Value ?? string.Empty;
        string email = claimsPrincipal.FindFirst("email")?.Value ?? string.Empty;
        string? emailVerifiedClaim = claimsPrincipal.FindFirst("email_verified")?.Value;
        bool emailVerified = string.Equals(emailVerifiedClaim, "true", StringComparison.OrdinalIgnoreCase);

        return new ExternalAuthPrincipal(
            ProviderUserId: subject,
            Email: email,
            EmailVerified: emailVerified,
            // Apple's id_token never carries given_name/family_name — the caller must forward
            // the one-time name payload Apple sends separately on first authorization, if any.
            FirstName: null,
            LastName: null
        );
    }

    private async Task<JsonWebKeySet> getAppleJwksAsync(CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(JwksCacheKey, out JsonWebKeySet? cached) && cached is not null)
            return cached;

        string json = await _httpClient.GetStringAsync(_options.JwksUri, cancellationToken);
        JsonWebKeySet jwks = new(json);

        _memoryCache.Set(JwksCacheKey, jwks, JwksCacheDuration);
        return jwks;
    }
}
