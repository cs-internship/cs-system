using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.EducationProgram;

namespace CrystallineSociety.Client.Shared.Pages;
public partial class EducationProgramPage
{
    private List<EducationProgramDto> EducationPrograms = new();
    private bool IsSyncing = false;


    protected override async Task OnInitAsync()
    {
        // Todo : complete this code to return dto not entity.
        EducationPrograms = await HttpClient.GetFromJsonAsync<List<EducationProgramDto>>("BadgeSystem/GetEducationPrograms");
        await base.OnInitAsync();
    }

    private async Task HandelSyncAsync(EducationProgramDto educationProgram)
    {
        IsSyncing = true;
        await HttpClient.PostAsJsonAsync("BadgeSystem/SyncEducationProgramBadges", educationProgram);
        IsSyncing = false;
    }
}
