using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Client.Core.Components.Pages.Home;

public partial class HomePage
{
    [CascadingParameter] private BitDir? currentDir { get; set; }
    [AutoInject] private ICrystaProgramController CrystaProgramController { get; set; } = default!;

    private bool isLoading;
    private List<CrystaProgramDto> programs = [];

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        await LoadPrograms();
    }

    private async Task LoadPrograms()
    {
        try
        {
            isLoading = true;
            programs = await CrystaProgramController.GetPrograms(CurrentCancellationToken);

        }
        finally
        {
            isLoading = false;
        }
    }

    private void NavigateToDocuments(string programCode)
    {
        //PubSubService.Publish(ClientPubSubMessages.SET_PROGRAM_CODE, programCode);
        NavigationManager.NavigateTo(@Urls.Crysta.Program(programCode).DocsPage);
    }
}
