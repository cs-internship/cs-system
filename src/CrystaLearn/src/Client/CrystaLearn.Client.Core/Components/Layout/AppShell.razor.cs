using CrystaLearn.Shared.Dtos.Crysta;
using Microsoft.AspNetCore.Components.Routing;

namespace CrystaLearn.Client.Core.Components.Layout;

public partial class AppShell
{
    [Parameter] public bool? IsIdentityPage { get; set; }
    [Parameter] public bool IsDocPage { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [AutoInject] private IAppUpdateService appUpdateService = default!;
    [AutoInject] private SignInModalService signInModalService = default!;
    [AutoInject] private DocumentService documentService = default!;

    private bool isNavPanelOpen;
    private bool isNavPanelToggled;
    private List<BitNavItem> navPanelItems = [];
    private readonly List<Action> unsubscribers = [];

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        NavigationManager.LocationChanged += NavigationManager_LocationChanged;

        unsubscribers.Add(PubSubService.Subscribe(ClientPubSubMessages.OPEN_NAV_PANEL, async _ =>
        {
            isNavPanelOpen = true;
            isNavPanelToggled = false;
            StateHasChanged();
        }));

        unsubscribers.Add(PubSubService.Subscribe(ClientPubSubMessages.CLOSE_NAV_PANEL, async _ =>
        {
            isNavPanelOpen = false;
            isNavPanelToggled = false;
            StateHasChanged();
        }));

        unsubscribers.Add(PubSubService.Subscribe(ClientPubSubMessages.SET_PROGRAM_CODE, async (programCode) =>
        {
            navPanelItems = [];
            var programCodeStr = programCode as string ?? string.Empty;
            if (!string.IsNullOrEmpty(programCodeStr))
            {
                navPanelItems = await documentService.LoadNavItemsAsync(programCodeStr, CurrentCancellationToken);
            }
            StateHasChanged();
        }));
    }

    private string GetMainCssClass()
    {
        return IsIdentityPage is true ? "identity"
             : IsIdentityPage is false ? "non-identity"
             : string.Empty;
    }

    private async Task ModalSignIn()
    {
        await signInModalService.SignIn();
    }

    private async Task UpdateApp()
    {
        await appUpdateService.ForceUpdate();
    }

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // The sign-in and sign-up buttons href are bound to NavigationManager.GetRelativePath().
        // To ensure the bound values update with each route change, it's necessary to call StateHasChanged on location changes.
        StateHasChanged();
    }


    protected override async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing is false) return;

        unsubscribers.ForEach(us => us.Invoke());

        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;

        await base.DisposeAsync(disposing);
    }

    private async Task OnNavItemClicked(BitNavItem item)
    {
        var document = item.Data as DocumentDto;
        if (document is null)
        {
            return;
        }

        PubSubService.Publish(ClientPubSubMessages.SET_CURRENT_CRYSTA_URL, document.CrystaUrl);
    }
}
