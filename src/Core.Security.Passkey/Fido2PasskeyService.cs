using System.Text.Json;
using Fido2NetLib;
using Fido2NetLib.Objects;

namespace NArchitecture.Core.Security.Passkey;

public class Fido2PasskeyService : IPasskeyService
{
    private readonly IFido2 _fido2;

    public Fido2PasskeyService(IFido2 fido2)
    {
        _fido2 = fido2;
    }

    public string GenerateRegistrationOptions(
        byte[] userHandle,
        string userName,
        string userDisplayName,
        IEnumerable<byte[]> existingCredentialIds
    )
    {
        CredentialCreateOptions options = _fido2.RequestNewCredential(
            new RequestNewCredentialParams
            {
                User = new Fido2User
                {
                    Id = userHandle,
                    Name = userName,
                    DisplayName = userDisplayName
                },
                ExcludeCredentials = existingCredentialIds.Select(id => new PublicKeyCredentialDescriptor(id)).ToList(),
                AuthenticatorSelection = new AuthenticatorSelection
                {
                    ResidentKey = ResidentKeyRequirement.Required,
                    UserVerification = UserVerificationRequirement.Preferred
                },
                AttestationPreference = AttestationConveyancePreference.None
            }
        );

        return options.ToJson();
    }

    public async Task<Fido2CredentialDescriptor> VerifyRegistrationAsync(
        string attestationResponseJson,
        string registrationOptionsJson,
        Func<byte[], CancellationToken, Task<bool>> isCredentialIdUniqueToUser,
        CancellationToken cancellationToken = default
    )
    {
        AuthenticatorAttestationRawResponse attestationResponse =
            JsonSerializer.Deserialize<AuthenticatorAttestationRawResponse>(attestationResponseJson)
            ?? throw new ArgumentException("Geçersiz passkey kayıt cevabı.", nameof(attestationResponseJson));
        CredentialCreateOptions originalOptions = CredentialCreateOptions.FromJson(registrationOptionsJson);

        RegisteredPublicKeyCredential result = await _fido2.MakeNewCredentialAsync(
            new MakeNewCredentialParams
            {
                AttestationResponse = attestationResponse,
                OriginalOptions = originalOptions,
                IsCredentialIdUniqueToUserCallback = (p, ct) => isCredentialIdUniqueToUser(p.CredentialId, ct)
            },
            cancellationToken
        );

        return new Fido2CredentialDescriptor(result.Id, result.PublicKey, result.SignCount);
    }

    public string GenerateAssertionOptions(IEnumerable<byte[]>? allowCredentialIds)
    {
        AssertionOptions options = _fido2.GetAssertionOptions(
            new GetAssertionOptionsParams
            {
                AllowedCredentials = allowCredentialIds?.Select(id => new PublicKeyCredentialDescriptor(id)).ToList()
                    ?? new List<PublicKeyCredentialDescriptor>(),
                UserVerification = UserVerificationRequirement.Preferred
            }
        );

        return options.ToJson();
    }

    public AssertionIdentifiers ReadAssertionIdentifiers(string assertionResponseJson)
    {
        AuthenticatorAssertionRawResponse assertionResponse =
            JsonSerializer.Deserialize<AuthenticatorAssertionRawResponse>(assertionResponseJson)
            ?? throw new ArgumentException("Geçersiz passkey giriş cevabı.", nameof(assertionResponseJson));

        return new AssertionIdentifiers(assertionResponse.RawId, assertionResponse.Response.UserHandle ?? Array.Empty<byte>());
    }

    public async Task<(byte[] CredentialId, uint NewSignCount)> VerifyAssertionAsync(
        string assertionResponseJson,
        string assertionOptionsJson,
        Fido2CredentialDescriptor storedCredential,
        Func<byte[], byte[], CancellationToken, Task<bool>> isUserHandleOwnerOfCredentialId,
        CancellationToken cancellationToken = default
    )
    {
        AuthenticatorAssertionRawResponse assertionResponse =
            JsonSerializer.Deserialize<AuthenticatorAssertionRawResponse>(assertionResponseJson)
            ?? throw new ArgumentException("Geçersiz passkey giriş cevabı.", nameof(assertionResponseJson));
        AssertionOptions originalOptions = AssertionOptions.FromJson(assertionOptionsJson);

        VerifyAssertionResult result = await _fido2.MakeAssertionAsync(
            new MakeAssertionParams
            {
                AssertionResponse = assertionResponse,
                OriginalOptions = originalOptions,
                StoredPublicKey = storedCredential.PublicKey,
                StoredSignatureCounter = storedCredential.SignCount,
                IsUserHandleOwnerOfCredentialIdCallback = (p, ct) =>
                    isUserHandleOwnerOfCredentialId(p.CredentialId, p.UserHandle, ct)
            },
            cancellationToken
        );

        return (result.CredentialId, result.SignCount);
    }
}
