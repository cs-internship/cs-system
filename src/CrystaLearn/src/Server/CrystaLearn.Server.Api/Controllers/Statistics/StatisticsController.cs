using CrystaLearn.Server.Api.Services;
using CrystaLearn.Shared.Dtos.Statistics;
using CrystaLearn.Shared.Controllers.Statistics;

namespace CrystaLearn.Server.Api.Controllers.Statistics;

[ApiController, Route("api/[controller]/[action]")]
public partial class StatisticsController : AppControllerBase, IStatisticsController
{
    [AutoInject] private NugetStatisticsHttpClient nugetHttpClient = default!;

    [AllowAnonymous]
    [HttpGet("{packageId}")]
    [ResponseCache(Duration = 1 * 24 * 3600, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "*" })]
    public async Task<NugetStatsDto> GetNugetStats(string packageId, CancellationToken cancellationToken)
    {
        return await nugetHttpClient.GetPackageStats(packageId, cancellationToken);
    }
}
