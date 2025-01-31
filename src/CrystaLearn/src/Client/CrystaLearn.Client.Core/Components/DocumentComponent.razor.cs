using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Client.Core.Components;

public partial class DocumentComponent
{
    [AutoInject]
    private IDocumentController DocumentController { get; set; } = default!;

    [Parameter] public DocumentDto? Document { get; set; } = default!;


    private DocumentDto? LoadedDocument { get; set; }
    private bool IsLoadingDocument { get; set; } = false;
    
    [AutoInject]
    private MessageBoxService MessageBoxService { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        if (Document is not null && Document.CrystaProgram is not null)
        {
            try
            {
                IsLoadingDocument = true;
                var programCode = Document.CrystaProgram.Code;
                LoadedDocument = await DocumentController.GetDocumentByCode(programCode, Document.Code, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(ex.Message, "Error");
            }
            finally
            {
                IsLoadingDocument = false;
            }
        }
        await base.OnParametersSetAsync();
    }
}
