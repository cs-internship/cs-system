using CrystaLearn.Shared.Dtos.Dashboard;

namespace CrystaLearn.Shared.Controllers.Dashboard;

[Route("api/[controller]/[action]/"), AuthorizedApi]
public interface IDashboardController : IAppController
{
    [HttpGet]
    Task<OverallAnalyticsStatsDataResponseDto> GetOverallAnalyticsStatsData(CancellationToken cancellationToken);
}
