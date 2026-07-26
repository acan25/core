# NArchitecture.Core.Security.Passkey

Storage-agnostic WebAuthn/Passkey (FIDO2) registration and assertion services, built on
[Fido2NetLib](https://github.com/passwordless-lib/fido2-net-lib). Kept as its own package
(separate from `Core.Security`) so that projects which don't need passkey support aren't
forced to take the Fido2NetLib dependency tree.

Call `services.AddPasskeyServices(passkeyOptions)` to register `IPasskeyService`.
