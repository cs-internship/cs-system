using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Shared.Components;
public partial class ProgramDocumentTree
{
    [Parameter] public List<ProgramDocumentDto> ProgramDocuments { get; set; } = new();
    [Parameter] public EventCallback<ProgramDocumentDto> ProgramDocumentCallBack { get; set; }

    private string? ActiveProgramDocumentUrl { get; set; }

    private async Task OnProgramDocumentClick(ProgramDocumentDto programDocument)
    {
        ActiveProgramDocumentUrl = programDocument.Url;
        await ProgramDocumentCallBack.InvokeAsync(programDocument);
    }
}
