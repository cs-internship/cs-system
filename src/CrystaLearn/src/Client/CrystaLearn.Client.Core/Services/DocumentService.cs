using CrystaLearn.Shared.Controllers.Crysta;

namespace CrystaLearn.Client.Core.Services;
public partial class DocumentService
{
    [AutoInject] private IDocumentController DocumentController { get; set; } = default!;

    public async Task<List<BitNavItem>> LoadNavItemsAsync(string programCode, CancellationToken cancellationToken)
    {
        var docs = await DocumentController.GetDocuments(programCode, cancellationToken);
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

        return root.ChildItems;
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
}
