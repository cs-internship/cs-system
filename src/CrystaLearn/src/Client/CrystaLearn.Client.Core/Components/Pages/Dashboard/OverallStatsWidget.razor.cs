using CrystaLearn.Shared.Controllers.Dashboard;
using CrystaLearn.Shared.Dtos.Dashboard;

namespace CrystaLearn.Client.Core.Components.Pages.Dashboard;

public partial class OverallStatsWidget
{
    [AutoInject] IDashboardController dashboardController = default!;

    private bool isLoading;
    private OverallAnalyticsStatsDataResponseDto dto = new();

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        await GetData();
    }

    private async Task GetData()
    {
        isLoading = true;

        try
        {
            dto = await dashboardController.GetOverallAnalyticsStatsData(CurrentCancellationToken);
        }
        finally
        {
            isLoading = false;
        }
    }
}
