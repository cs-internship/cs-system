using System.Collections;
using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Controllers.Statistics;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Statistics;

namespace CrystaLearn.Client.Core.Components.Pages;

public partial class HomePage
{
    protected override string? Title => Localizer[nameof(AppStrings.Home)];
    protected override string? Subtitle => string.Empty;

    [CascadingParameter] private BitDir? currentDir { get; set; }

    [AutoInject] private IStatisticsController statisticsController = default!;
    [AutoInject] private ICrystaProgramController CrystaProgramController { get; set; } = default!;
    private bool isLoadingGitHub = true;
    private bool isLoadingNuget = true;
    private GitHubStats? gitHubStats;
    private NugetStatsDto? nugetStats;
    private List<CrystaProgramDto> Programs { get; set; } = [];

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        // If required, you should typically manage the authorization header for external APIs in **AuthDelegatingHandler.cs**
        // and handle error extraction from failed responses in **ExceptionDelegatingHandler.cs**.  

        // These external API calls are provided as sample references for anonymous API usage in pre-rendering anonymous pages,
        // and comprehensive exception handling is not intended for these examples.  

        // However, the logic in other HTTP message handlers, such as **LoggingDelegatingHandler** and **RetryDelegatingHandler**,
        // effectively addresses most scenarios.

        await Task.WhenAll(LoadNuget(), LoadGitHub(), LoadPrograms());
    }

    private async Task LoadPrograms()
    {
        Programs = await CrystaProgramController.GetPrograms(CurrentCancellationToken);
    }

    private async Task LoadNuget()
    {
        try
        {
            nugetStats = await statisticsController.GetNugetStats(packageId: "Bit.BlazorUI", CurrentCancellationToken);
        }
        finally
        {
            isLoadingNuget = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task LoadGitHub()
    {
        try
        {
            // GitHub results (2nd Bit Pivot tab) aren't shown by default and aren't critical for SEO,
            // so we can skip it in pre-rendering to save time.
            if (InPrerenderSession is false)
            {
                gitHubStats = await statisticsController.GetGitHubStats(CurrentCancellationToken);
            }
        }
        catch
        {
            // GetGitHubStats method calls the GitHub API directly from the client.
            // We've intentionally ignored proper exception handling to keep this example simple. 
        }
        finally
        {
            isLoadingGitHub = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}
