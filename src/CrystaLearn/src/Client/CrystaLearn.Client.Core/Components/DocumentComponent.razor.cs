using System.Text.RegularExpressions;
using Bit.BlazorUI;
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

    [Parameter] public string? CrystaUrl { get; set; } = default!;
    private string? PreviousCrystaUrl { get; set; }

    private DocumentDto? LoadedDocument { get; set; }
    private bool IsLoadingDocument { get; set; } = false;
    private string? SelectedCulture { get; set; }
    [AutoInject]
    private MessageBoxService MessageBoxService { get; set; } = default!;

    private List<BitButtonGroupItem> MenuItems { get; set; } =
    [
        new() { Text = "Copy Link", IconName = BitIconName.Share },
        new() { Text = "GitHub", IconName = BitIconName.GitGraph },
    ];

    public List<BitBreadcrumbItem> BreadcrumbItems { get; set; } = [];

    protected override async Task OnParamsSetAsync()
    {
        await base.OnParamsSetAsync();
        await LoadDocument();
    }

    private async Task LoadDocument()
    {
        try
        {
            IsLoadingDocument = true;
            
            if (CrystaUrl is not null)
            {
                if (PreviousCrystaUrl == CrystaUrl)
                {
                    return;
                }

                PreviousCrystaUrl = CrystaUrl;

                var culture = CultureInfo.CurrentUICulture.Name;
                LoadedDocument = await DocumentController.GetContentByCrystaUrl(CrystaUrl, culture, CancellationToken.None);
            }

            if (LoadedDocument != null)
            {
                var parts = $"{LoadedDocument.Folder?.Trim('/')}/{LoadedDocument.FileName}".Split('/');

                BreadcrumbItems = parts
                                  .Select((folder, index) => new BitBreadcrumbItem
                                  {
                                      Text = folder,
                                      IsEnabled = (index == parts.Length-1)
                                  })
                                  .ToList();
            }

            SelectedCulture = GetCulture(LoadedDocument)?.Culture.Name;
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

    private (string DisplayName, CultureInfo Culture)? GetCulture(DocumentDto? document)
    {
        if (document is null)
        {
            return null;
        }

        var culture = CultureInfoManager.SupportedCultures.FirstOrDefault(s=>s.Culture.Name.ToLower().StartsWith(document.Culture.ToLower()));

        return culture;
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

    private BitDropdownItem<string>[] GetLanguages(DocumentDto loadedDocument)
    {
        var cultures = CultureInfoManager.SupportedCultures
                                     .Select(sc => new BitDropdownItem<string> { Value = sc.Culture.Name, Text = sc.DisplayName })
                                     .Where(sc=>loadedDocument.CultureVariants.Any(v=>sc.Value?.ToLower().StartsWith(v.ToLower())??false))
                                     .ToArray();

        return cultures;
    }

    private string? GetCultureMessage(DocumentDto? document)
    {
        var culture = CultureInfo.CurrentUICulture;
        if (document?.CultureVariants.Any(v=> culture.Name.ToLower().StartsWith(v.ToLower())) ?? true)
        {
            return "Language:";
        }

        return $"Not in {culture.DisplayName}:";
    }

    private async Task OnCultureChanged(string? s)
    {
        //throw new NotImplementedException();
    }
}
