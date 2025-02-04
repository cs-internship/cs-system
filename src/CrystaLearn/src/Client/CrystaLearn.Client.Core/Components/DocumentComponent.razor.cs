using System.Text.RegularExpressions;
using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Services;

namespace CrystaLearn.Client.Core.Components;

public partial class DocumentComponent
{
    [AutoInject]
    private IDocumentController DocumentController { get; set; } = default!;
    [AutoInject]
    private CultureInfoManager CultureInfoManager { get; set; } = default!;

    [Parameter] public DocumentDto? Document { get; set; } = default!;
    private DocumentDto? PreviousDocument { get; set; }

    private DocumentDto? LoadedDocument { get; set; }
    private bool IsLoadingDocument { get; set; } = false;
    
    [AutoInject]
    private MessageBoxService MessageBoxService { get; set; } = default!;

    private List<BitButtonGroupItem> MenuItems { get; set; } =
    [
        new() { Text = "Copy Link", IconName = BitIconName.Share },
        new() { Text = "GitHub", IconName = BitIconName.GitGraph },
    ];

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await LoadDocument();
    }

    private async Task LoadDocument()
    {
        if (Document == PreviousDocument)
        {
            return;
        }

        PreviousDocument = Document;

        if (Document is not null && Document.CrystaProgram is not null)
        {
            try
            {
                IsLoadingDocument = true;

                if (Document.SourceHtmlUrl is not null)
                {
                    LoadedDocument = Document;
                    var doc = await DocumentController.GetDocumentContentByUrl(Document.SourceHtmlUrl, Document.CrystaProgram.Code, CancellationToken.None);
                    LoadedDocument.Content = doc.Content;
                }
                else
                {

                    var programCode = Document.CrystaProgram.Code;
                    var culture = CultureInfo.CurrentUICulture.Name;
                    LoadedDocument = await DocumentController.GetDocumentByCode(programCode, Document.Code, culture, CancellationToken.None);
                }
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
    }

    private string? GetCultureTitle(DocumentDto? document)
    {
        if (document is null)
        {
            return null;
        }

        var culture = CultureInfoManager.SupportedCultures.FirstOrDefault(s=>s.Culture.Name.StartsWith(document.Culture));

        return culture.DisplayName;
    }

    private BitDir GetCultureDir(DocumentDto? document)
    {
        if (document?.Content is null)
        {
            return BitDir.Ltr;
        }

        var content = document.Content;
        var isRtl = GitHubUtil.IsRtl(content);
        return isRtl ? BitDir.Rtl : BitDir.Ltr;
    }
}
