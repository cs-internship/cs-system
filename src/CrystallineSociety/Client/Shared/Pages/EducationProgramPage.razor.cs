using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Shared.Pages;
public partial class EducationProgramPage
{
    private List<OrganizationDto> Organizations = new();
    private bool IsSyncing = false;


    protected override async Task OnInitAsync()
    {
        // Todo : complete this code to return dto not entity.
        Organizations = await HttpClient.GetFromJsonAsync<List<OrganizationDto>>("Organization/GetOrganizations");
        await base.OnInitAsync();
    }

    private async Task HandelSyncAsync(OrganizationDto organization)
    {
        IsSyncing = true;
        await HttpClient.PostAsJsonAsync("Organization/SyncOrganizationBadges", organization);
        await HttpClient.PostAsJsonAsync("ProgramDocument/SyncOrganizationProgramDocuments", organization);
        IsSyncing = false;
    }
}
