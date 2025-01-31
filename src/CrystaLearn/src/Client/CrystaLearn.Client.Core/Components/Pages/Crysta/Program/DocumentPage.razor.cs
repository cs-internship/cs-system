using CrystaLearn.Shared.Controllers.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;

namespace CrystaLearn.Client.Core.Components.Pages.Crysta.Program;

public partial class DocumentPage
{
    [AutoInject] private IDocumentController DocumentController { get; set; } = default!;

    [Parameter]
    public string ProgramCode { get; set; } = default!;

    protected override string? Title => Localizer[nameof(AppStrings.Terms)];
    protected override string? Subtitle => string.Empty;
    public List<BitNavItem> DocumentsTree { get; set; } = [];
    public DocumentDto? CurrentDocument { get; set; }
    private bool IsLoadingTree { get; set; } = false;

    protected override async Task OnInitAsync()
    {
        await RefreshDocuments();

        await base.OnInitAsync();
    }

    private async Task RefreshDocuments()
    {
        try
        {
            IsLoadingTree = true;
            StateHasChanged();

            var docs = await DocumentController.GetDocuments(ProgramCode, CurrentCancellationToken);
            var root = new BitNavItem()
            {
                Text = "/",
            };

            foreach (var doc in docs)
            {
                var folderParts = doc.Folder?.Trim('/')?.Split('/') ?? [];
                folderParts = ["/", .. folderParts];

                var navItem = GetOrCreateNavItem(root, folderParts);

                navItem.ChildItems.Add(new BitNavItem
                {
                    Text = doc.Title,
                    Data = doc,
                    IconName = BitIconName.TextDocument
                });
            }

            DocumentsTree = root.ChildItems;
        }
        finally
        {
            IsLoadingTree = false;
            StateHasChanged();
        }
    }

    private BitNavItem GetOrCreateNavItem(BitNavItem root, string[] folderParts)
    {
        if (folderParts.Length == 0)
        {
            return root;
        }

        var folderPart = folderParts[0];

        BitNavItem? foundFolder = null;

        if (root.Text == folderPart)
        {
            foundFolder = root;
        }
        else
        {
            foreach (var child in root.ChildItems)
            {
                if (child.Text == folderPart)
                {
                    foundFolder = child;
                    // current = child;
                }
            }
        }

        if (foundFolder == null)
        {
            foundFolder = new BitNavItem
            {
                Text = folderPart,
                IconName = BitIconName.FabricFolder
            };
            root.ChildItems.Add(foundFolder);

        }

        return GetOrCreateNavItem(foundFolder, folderParts.Skip(1).ToArray());
    }

    private async Task OnRefreshClicked()
    {
        await RefreshDocuments();
    }
}
