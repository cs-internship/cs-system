using CrystaLearn.Shared.Dtos.Chatbot;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Identity;
using CrystaLearn.Shared.Dtos.PushNotification;
using CrystaLearn.Shared.Dtos.Statistics;
using Fido2NetLib;
using Fido2NetLib.Objects;

namespace CrystaLearn.Shared.Dtos;

/// <summary>
/// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Dictionary<string, JsonElement>))]
[JsonSerializable(typeof(Dictionary<string, string?>))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(Guid[]))]
[JsonSerializable(typeof(GitHubStats))]
[JsonSerializable(typeof(NugetStatsDto))]
[JsonSerializable(typeof(AppProblemDetails))]
[JsonSerializable(typeof(PushNotificationSubscriptionDto))]
[JsonSerializable(typeof(AssertionOptions))]
[JsonSerializable(typeof(AuthenticatorAssertionRawResponse))]
[JsonSerializable(typeof(AuthenticatorAttestationRawResponse))]
[JsonSerializable(typeof(CredentialCreateOptions))]
[JsonSerializable(typeof(VerifyAssertionResult))]
[JsonSerializable(typeof(VerifyWebAuthnAndSignInDto))]
[JsonSerializable(typeof(WebAuthnAssertionOptionsRequestDto))]
[JsonSerializable(typeof(DocumentDto))]
[JsonSerializable(typeof(List<DocumentDto>))]
[JsonSerializable(typeof(CrystaProgramDto))]
[JsonSerializable(typeof(CrystaProgramLightDto))]
[JsonSerializable(typeof(List<CrystaProgramDto>))]
[JsonSerializable(typeof(SyncInfoDto))]

[JsonSerializable(typeof(UpdateSystemPromptDto))]
public partial class AppJsonContext : JsonSerializerContext
{
}
