namespace CrystallineSociety.Client.Core.Components;
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
