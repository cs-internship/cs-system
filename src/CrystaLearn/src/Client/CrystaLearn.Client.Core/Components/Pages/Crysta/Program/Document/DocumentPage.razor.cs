using CrystaLearn.Shared.Controllers.Crysta;

namespace CrystaLearn.Client.Core.Components.Pages.Crysta.Program.Document;

public partial class DocumentPage : IAsyncDisposable
{
    [AutoInject] private IDocumentController DocumentController { get; set; } = default!;
    [AutoInject] private PubSubService pubSubService = default!;

    [Parameter] public string ProgramCode { get; set; } = default!;
    [Parameter] public string? DocPath { get; set; } = default!;


    protected override string? Title => Localizer[nameof(AppStrings.Terms)];
    protected override string? Subtitle => string.Empty;
    public List<BitNavItem> DocumentsTree { get; set; } = [];
    public string? CurrentCrystaUrl { get; set; }
    private bool isLoadingTree = false;
    private List<Action> unsubscribers = [];


    protected override async Task OnInitAsync()
    {
        if (!string.IsNullOrWhiteSpace(DocPath))
        {
            CurrentCrystaUrl = Urls.Crysta.Program(ProgramCode).DocPage(DocPath);
        }

        await RefreshDocuments();
        pubSubService.Publish(ClientPubSubMessages.SHOW_NAV_PANEL, DocumentsTree);
        unsubscribers.Add(pubSubService.Subscribe(ClientPubSubMessages.SET_CURRENT_CRYSTA_URL, async (url) =>
        {
            CurrentCrystaUrl = url as string;
            StateHasChanged();
        }));
        await base.OnInitAsync();
    }

    private async Task RefreshDocuments()
    {
        try
        {
            isLoadingTree = true;
            StateHasChanged();

            var docs = await DocumentController.GetDocuments(ProgramCode, CurrentCancellationToken);
            var root = new BitNavItem()
            {
                Text = "/",
            };

            foreach (var doc in docs)
            {
                var folderParts =
                    doc.Folder?
                    .Trim('/')?
                    .Split('/')?
                    .Where(s => !string.IsNullOrWhiteSpace(s))?
                    .ToArray()
                    ?? [];
                folderParts = ["/", .. folderParts];

                var navItem = GetOrCreateNavItem(root, folderParts);

                navItem.ChildItems.Add(new BitNavItem
                {
                    Text = doc.Title,
                    Data = doc,
                    IconName = BitIconName.TextDocument
                });
            }

            var allNavItems = root.ChildItems.SelectMany(n => new List<BitNavItem>([n, .. (n.ChildItems)])).ToList();
            allNavItems.Add(root);

            foreach (var item in allNavItems)
            {
                item.ChildItems = item.ChildItems.OrderBy(i => $"{(i.Data is null ? "0-folder/" : "1-file/")}{i.Text}").ToList();
            }

            DocumentsTree = root.ChildItems;
        }
        finally
        {
            isLoadingTree = false;
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

    public async ValueTask DisposeAsync()
    {
        unsubscribers.ForEach(us => us.Invoke());
    }
}
