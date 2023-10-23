using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.EducationProgram;

namespace CrystallineSociety.Client.Shared.Pages;
public partial class EducationProgramPage
{
    private List<EducationProgramDto> educationPrograms = new();


    protected override async Task OnInitAsync()
    {
        // Todo : complete this code to return dto not entity.
        educationPrograms = await HttpClient.GetFromJsonAsync<List<EducationProgramDto>>("BadgeSystem/GetEducationPrograms");
        await HttpClient.PostAsJsonAsync("BadgeSystem/SyncEducationProgramBadges", educationPrograms);
        await base.OnInitAsync();
    }
}
