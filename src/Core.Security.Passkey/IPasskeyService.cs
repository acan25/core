namespace NArchitecture.Core.Security.Passkey;

/// <summary>
/// Storage-agnostic WebAuthn/Passkey ceremony helper. Callers own persistence of <see cref="Fido2CredentialDescriptor"/>
/// and challenge-options JSON (e.g. via IDistributedCache keyed by a short-lived id).
/// </summary>
public interface IPasskeyService
{
    /// <param name="userHandle">Stable per-user identifier (e.g. the raw bytes of the user's Guid) — returned back in assertions.</param>
    /// <param name="existingCredentialIds">Credential ids already registered to this user, excluded from re-registration.</param>
    string GenerateRegistrationOptions(
        byte[] userHandle,
        string userName,
        string userDisplayName,
        IEnumerable<byte[]> existingCredentialIds
    );

    /// <param name="isCredentialIdUniqueToUser">Callback the caller implements against its own storage to reject credential id collisions.</param>
    Task<Fido2CredentialDescriptor> VerifyRegistrationAsync(
        string attestationResponseJson,
        string registrationOptionsJson,
        Func<byte[], CancellationToken, Task<bool>> isCredentialIdUniqueToUser,
        CancellationToken cancellationToken = default
    );

    /// <param name="allowCredentialIds">Null/empty for usernameless (discoverable-credential) login.</param>
    string GenerateAssertionOptions(IEnumerable<byte[]>? allowCredentialIds);

    /// <summary>
    /// Reads the credential id and user handle out of a raw (unverified) assertion response, so the caller
    /// can look up the stored credential/user before running the actual crypto verification.
    /// </summary>
    AssertionIdentifiers ReadAssertionIdentifiers(string assertionResponseJson);

    /// <param name="isUserHandleOwnerOfCredentialId">Callback the caller implements against its own storage.</param>
    Task<(byte[] CredentialId, uint NewSignCount)> VerifyAssertionAsync(
        string assertionResponseJson,
        string assertionOptionsJson,
        Fido2CredentialDescriptor storedCredential,
        Func<byte[], byte[], CancellationToken, Task<bool>> isUserHandleOwnerOfCredentialId,
        CancellationToken cancellationToken = default
    );
}
