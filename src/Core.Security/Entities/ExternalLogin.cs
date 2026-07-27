using NArchitecture.Core.Persistence.Repositories;
using NArchitecture.Core.Security.Enums;

namespace NArchitecture.Core.Security.Entities;

public class ExternalLogin<TId, TUserId> : Entity<TId>
{
    public TUserId UserId { get; set; }
    public ExternalAuthProvider Provider { get; set; }
    public string ProviderUserId { get; set; }
    public string Email { get; set; }
    public bool EmailVerifiedByProvider { get; set; }
    public DateTime LinkedAt { get; set; }

    public ExternalLogin()
    {
        UserId = default!;
        ProviderUserId = string.Empty;
        Email = string.Empty;
    }

    public ExternalLogin(
        TUserId userId,
        ExternalAuthProvider provider,
        string providerUserId,
        string email,
        bool emailVerifiedByProvider
    )
    {
        UserId = userId;
        Provider = provider;
        ProviderUserId = providerUserId;
        Email = email;
        EmailVerifiedByProvider = emailVerifiedByProvider;
        LinkedAt = DateTime.UtcNow;
    }

    public ExternalLogin(
        TId id,
        TUserId userId,
        ExternalAuthProvider provider,
        string providerUserId,
        string email,
        bool emailVerifiedByProvider
    )
        : base(id)
    {
        UserId = userId;
        Provider = provider;
        ProviderUserId = providerUserId;
        Email = email;
        EmailVerifiedByProvider = emailVerifiedByProvider;
        LinkedAt = DateTime.UtcNow;
    }
}
