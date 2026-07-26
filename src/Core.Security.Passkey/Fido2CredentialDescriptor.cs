namespace NArchitecture.Core.Security.Passkey;

/// <summary>
/// The parts of a registered passkey a caller needs to persist and later present back for assertion verification.
/// </summary>
public record Fido2CredentialDescriptor(byte[] CredentialId, byte[] PublicKey, uint SignCount);

/// <summary>
/// Identifiers read off a raw assertion response before crypto verification — lets the caller look up
/// which user/credential to load (usernameless/discoverable login) before calling <see cref="IPasskeyService.VerifyAssertionAsync"/>.
/// </summary>
public record AssertionIdentifiers(byte[] CredentialId, byte[] UserHandle);
