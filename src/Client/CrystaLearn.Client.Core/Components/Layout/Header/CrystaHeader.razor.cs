using CrystaLearn.Client.Core.Services;
using CrystaLearn.Shared.Controllers.Identity;
using CrystaLearn.Shared.Dtos.Identity;
using Microsoft.AspNetCore.Components.Routing;

namespace CrystaLearn.Client.Core.Components.Layout.Header;

public partial class CrystaHeader : AppComponentBase
{
    [CascadingParameter] public BitDir? CurrentDir { get; set; }
    [CascadingParameter] public AppThemeType? CurrentTheme { get; set; }

    [AutoInject] private History history = default!;
    [AutoInject] private ThemeService themeService = default!;
    [AutoInject] private SignInModalService signInModalService = default!;
    [AutoInject] private CultureService cultureService = default!;
    [AutoInject] private readonly IUserController userController = default!;
    [AutoInject] private readonly ITelemetryContext telemetryContext = default!;
    [AutoInject] private PubSubService pubSubService { get; set; } = default!;
    private BitDropdownItem<string>[] cultures = default!;

    private bool isDarkMode => CurrentTheme == AppThemeType.Dark;


    private string? pageTitle;
    private string? pageSubtitle;
    private bool showGoBackButton;
    private bool isOpen;
    private bool isAuthenticated;
    private bool isSignOutConfirmOpen;
    private bool isMenuPanelOpen = false;
    private Action unsubscribePageTitleChanged = default!;

    private UserDto user = new();

    private Uri absoluteServerAddress => new(NavigationManager.BaseUri);

    private bool ShowLanguages { get; set; }
    protected override async Task OnInitAsync()
    {
        AuthManager.AuthenticationStateChanged += AuthManager_AuthenticationStateChanged;

        await base.OnInitAsync();
        if (CultureInfoManager.InvariantGlobalization is false)
        {
            cultures = CultureInfoManager.SupportedCultures
                              .Select(sc => new BitDropdownItem<string> { Value = sc.Culture.Name, Text = sc.DisplayName })
                              .ToArray();
        }
        unsubscribePageTitleChanged = PubSubService.Subscribe(ClientPubSubMessages.PAGE_DATA_CHANGED, async payload =>
        {
            (pageTitle, pageSubtitle, showGoBackButton) = ((string?, string?, bool))payload!;

            StateHasChanged();
        });

        NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        await GetCurrentUser(AuthenticationStateTask);
    }


    private void OpenNavPanel()
    {
        PubSubService.Publish(ClientPubSubMessages.OPEN_NAV_PANEL);
    }

    private async Task GoBack()
    {
        await history.GoBack();
    }

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // The sign-in and sign-up button hrefs are bound to NavigationManager.GetRelativePath().
        // To ensure the bound values update with each route change, it's necessary to call StateHasChanged on location changes.
        StateHasChanged();
    }


    protected override async ValueTask DisposeAsync(bool disposing)
    {
        await base.DisposeAsync(disposing);

        unsubscribePageTitleChanged?.Invoke();
        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
    }

    private async Task ToggleTheme()
    {
        await themeService.ToggleTheme();
    }

    private void OnLogoClicked()
    {
        NavigationManager.NavigateTo("/");
    }
    private async Task OpenMenuPanel()
    {
        isMenuPanelOpen = !isMenuPanelOpen;
    }

    private async Task OnCultureChanged(string? cultureName)
    {
        await cultureService.ChangeCulture(cultureName);
    }

    private readonly List<BitMenuButtonItem> dirItems = new()
    {
        new() { Text = "Country", Key = "Country" },
        new() { Text = "State", Key = "State" },
        new() { Text = "Municipality", Key = "Municipality" },
        new() { Text = "Community", Key = "Community" },
        new() { Text = "Street", Key = "Street" },
        new() { Text = "Unit", Key = "Unit" }
    };

    private async Task ModalSignIn()
    {
        PubSubService.Publish(ClientPubSubMessages.SIGNIN_BUTTON_CLICKED);
        isOpen = false;
        await signInModalService.SignIn();
    }

    private async Task SignOut()
    {
        isOpen = false;
        await AuthManager.SignOut(CurrentCancellationToken);
    }

    private async Task GetCurrentUser(Task<AuthenticationState> task)
    {
        isAuthenticated = (await task).User.IsAuthenticated();
        try
        {
            if (isAuthenticated)
            {
                user = await userController.GetCurrentUser(CurrentCancellationToken);
            }
        }
        catch (UnauthorizedException)
        {
            //ignore
        }
    }

    private async void AuthManager_AuthenticationStateChanged(Task<AuthenticationState> task)
    {
        try
        {
            await GetCurrentUser(task);
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private void HandleShowLanguages() => ShowLanguages = true;
    private void SlideBack() => ShowLanguages = false;
}
