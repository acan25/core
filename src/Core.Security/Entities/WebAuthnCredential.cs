using NArchitecture.Core.Persistence.Repositories;

namespace NArchitecture.Core.Security.Entities;

public class WebAuthnCredential<TId, TUserId> : Entity<TId>
{
    public TUserId UserId { get; set; }
    public byte[] CredentialId { get; set; }
    public byte[] PublicKey { get; set; }
    public uint SignCount { get; set; }
    public string DeviceName { get; set; }
    public DateTime? LastUsedAt { get; set; }

    public WebAuthnCredential()
    {
        UserId = default!;
        CredentialId = Array.Empty<byte>();
        PublicKey = Array.Empty<byte>();
        DeviceName = string.Empty;
    }

    public WebAuthnCredential(TUserId userId, byte[] credentialId, byte[] publicKey, uint signCount, string deviceName)
    {
        UserId = userId;
        CredentialId = credentialId;
        PublicKey = publicKey;
        SignCount = signCount;
        DeviceName = deviceName;
    }

    public WebAuthnCredential(
        TId id,
        TUserId userId,
        byte[] credentialId,
        byte[] publicKey,
        uint signCount,
        string deviceName
    )
        : base(id)
    {
        UserId = userId;
        CredentialId = credentialId;
        PublicKey = publicKey;
        SignCount = signCount;
        DeviceName = deviceName;
    }
}
