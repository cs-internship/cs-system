using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Core.Components.Pages;
public partial class OrganizationsSettingsPage
{
    private List<OrganizationDto> Organizations = new();
    private bool IsSyncing = false;
    private bool IsDeleting = false;


    protected override async Task OnInitAsync()
    {
        // Todo : complete this code to return dto not entity.
        Organizations = await PrerenderStateService.GetValue(async () => await HttpClient.GetFromJsonAsync<List<OrganizationDto>>("api/Organization/GetOrganizations")) ?? new();
        await base.OnInitAsync();
    }

    private async Task HandelSyncAsync(OrganizationDto organization)
    {
        IsSyncing = true;
        await HttpClient.PostAsJsonAsync("api/Organization/SyncOrganizationBadges", organization);
        await HttpClient.PostAsJsonAsync("api/ProgramDocument/SyncOrganizationProgramDocuments", organization);
        IsSyncing = false;
    }

        private async Task HandelDeleteAsync(OrganizationDto organization)
    {
        IsDeleting = true;
        await HttpClient.PostAsJsonAsync("api/Organization/SyncOrganizationBadges", organization);
        await HttpClient.PostAsJsonAsync("api/ProgramDocument/SyncOrganizationProgramDocuments", organization);
        IsDeleting = false;
    }
}
