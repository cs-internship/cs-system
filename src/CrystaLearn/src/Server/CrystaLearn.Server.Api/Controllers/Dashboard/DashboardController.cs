using CrystaLearn.Shared.Controllers.Dashboard;
using CrystaLearn.Shared.Dtos.Dashboard;

namespace CrystaLearn.Server.Api.Controllers.Dashboard;

[ApiController, Route("api/[controller]/[action]"),
    Authorize(Policy = AuthPolicies.PRIVILEGED_ACCESS),
    Authorize(Policy = AppFeatures.AdminPanel.Dashboard)]
public partial class DashboardController : AppControllerBase, IDashboardController
{
    [HttpGet]
    public async Task<OverallAnalyticsStatsDataResponseDto> GetOverallAnalyticsStatsData(CancellationToken cancellationToken)
    {
        var result = new OverallAnalyticsStatsDataResponseDto();

        var last30DaysDate = DateTimeOffset.UtcNow.AddDays(-30);

        return result;
    }
}
