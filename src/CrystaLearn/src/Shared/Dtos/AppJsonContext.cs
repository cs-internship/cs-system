using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.PushNotification;
using CrystaLearn.Shared.Dtos.Statistics;

namespace CrystaLearn.Shared.Dtos;

/// <summary>
/// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Dictionary<string, JsonElement>))]
[JsonSerializable(typeof(Dictionary<string, string?>))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(RestErrorInfo))]
[JsonSerializable(typeof(GitHubStats))]
[JsonSerializable(typeof(NugetStatsDto))]
[JsonSerializable(typeof(PushNotificationSubscriptionDto))]
[JsonSerializable(typeof(DocumentDto))]
[JsonSerializable(typeof(List<DocumentDto>))]
public partial class AppJsonContext : JsonSerializerContext
{
}
