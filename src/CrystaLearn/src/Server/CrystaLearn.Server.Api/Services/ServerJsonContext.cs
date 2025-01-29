using CrystaLearn.Shared.Dtos.Statistics;

namespace CrystaLearn.Server.Api.Services;

/// <summary>
/// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(NugetStatsDto))]
public partial class ServerJsonContext : JsonSerializerContext
{
}
