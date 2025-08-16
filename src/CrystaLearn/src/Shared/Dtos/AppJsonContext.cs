using CrystaLearn.Shared.Dtos.Chatbot;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Dashboard;
using CrystaLearn.Shared.Dtos.Diagnostic;
using CrystaLearn.Shared.Dtos.Identity;
using CrystaLearn.Shared.Dtos.PushNotification;
using CrystaLearn.Shared.Dtos.Statistics;

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
[JsonSerializable(typeof(SendNotificationToRoleDto))]
[JsonSerializable(typeof(PushNotificationSubscriptionDto))]
[JsonSerializable(typeof(List<ProductsCountPerCategoryResponseDto>))]
[JsonSerializable(typeof(OverallAnalyticsStatsDataResponseDto))]
[JsonSerializable(typeof(List<ProductPercentagePerCategoryResponseDto>))]
[JsonSerializable(typeof(VerifyWebAuthnAndSignInRequestDto))]
[JsonSerializable(typeof(WebAuthnAssertionOptionsRequestDto))]
[JsonSerializable(typeof(DiagnosticLogDto[]))]
[JsonSerializable(typeof(StartChatbotRequest))]
[JsonSerializable(typeof(SystemPromptDto))]
[JsonSerializable(typeof(DocumentDto))]
[JsonSerializable(typeof(List<DocumentDto>))]
[JsonSerializable(typeof(CrystaProgramDto))]
[JsonSerializable(typeof(CrystaProgramLightDto))]
[JsonSerializable(typeof(List<CrystaProgramDto>))]
[JsonSerializable(typeof(SyncInfoDto))]
public partial class AppJsonContext : JsonSerializerContext
{
}
