using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Controllers.Statistics;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Client.Core.Components.Pages.Home;

public partial class HomePage
{
    [CascadingParameter] private BitDir? currentDir { get; set; }
    [AutoInject] private ICrystaProgramController CrystaProgramController { get; set; } = default!;

    private List<CrystaProgramDto> programs = [];

    [AutoInject] private IStatisticsController statisticsController = default!;


    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        await LoadPrograms();
    }

    private async Task LoadPrograms()
    {
        programs = await CrystaProgramController.GetPrograms(CurrentCancellationToken);
    }

    private void NavigateToDocuments(string programCode)
    {
        NavigationManager.NavigateTo(@Urls.Crysta.Program(programCode).DocsPage);
    }
}
