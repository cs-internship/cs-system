using CrystaLearn.Client.Core.Models;
using CrystaLearn.Client.Core.Services;
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

    private BitAppShell? _appShellRef;
    private bool isNavPanelOpen;
    private bool isNavPanelToggled;
    private bool isOpenButtonShit;
    private BitNavItem? selectedItem;
    private List<BitNavItem> navPanelItems = [];
    private readonly List<Action> unsubscribers = [];
    private ComponentMetadata? panelComponentMetadata { get; set; }

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

        unsubscribers.Add(PubSubService.Subscribe(ClientPubSubMessages.OPEN_BUTTON_SHIT, async dynamic =>
        {
            panelComponentMetadata = (ComponentMetadata)dynamic!;
            isOpenButtonShit = true;
            await InvokeAsync(StateHasChanged);
        }));

        unsubscribers.Add(PubSubService.Subscribe(ClientPubSubMessages.CLOSE_BUTTON_SHIT, async _ =>
        {
            isOpenButtonShit = false;
            await InvokeAsync(StateHasChanged);
        }));

        unsubscribers.Add(PubSubService.Subscribe(ClientPubSubMessages.SET_PROGRAM_CODE, async (payload) =>
        {
            navPanelItems = [];
            var payloadValue = payload as InitNavPayload;
            if (payloadValue is not null && !string.IsNullOrEmpty(payloadValue.ProgramCode))
            {
                navPanelItems = await documentService.LoadNavItemsAsync(payloadValue.ProgramCode, CurrentCancellationToken);
                selectedItem = !string.IsNullOrEmpty(payloadValue.CurrentCrystaUrl) ? documentService.FindNavItem(navPanelItems, payloadValue.CurrentCrystaUrl) : navPanelItems.FirstOrDefault();
                StateHasChanged();
            }
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

    private async Task OnSelectNavItem(BitNavItem item)
    {
        var document = item.Data as DocumentDto;
        if (document is null)
        {
            return;
        }

        PubSubService.Publish(ClientPubSubMessages.SET_CURRENT_CRYSTA_URL, document.CrystaUrl);
    }
}
