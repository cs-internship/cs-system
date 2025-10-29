using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Routing;
using CrystaLearn.Client.Core.Components.Pages.Identity.SignIn;

namespace CrystaLearn.Client.Core.Services;

/// <summary>
/// When users opt for social sign-in, they are seamlessly authenticated, whether they are new or returning users.
/// To provide a similarly streamlined experience for email/password sign-in, this service displays a modal dialog, enabling users to log in quickly without leaving the current page.
/// For optimal use of this service, it is recommended to remove the sign-up page and its associated links from the project entirely.
/// Optionally, you may also eliminate the password field from the sign-in form to allow users to authenticate solely via phone/OTP or email/magic-link.
/// </summary>
public partial class SignInModalService : IAsyncDisposable
{
    public SignInModalService(NavigationManager navigationManager,
                             PubSubService pubSubService,
                             IStringLocalizer<AppStrings> Localizer)
    {
        this.navigationManager = navigationManager;
        this.pubSubService = pubSubService;
        this.Localizer = Localizer;
        this.navigationManager.LocationChanged += NavigationManager_LocationChanged;
    }

    private NavigationManager navigationManager;
    private readonly PubSubService pubSubService;
    private TaskCompletionSource<bool>? signInPanelTcs;
    private IStringLocalizer<AppStrings> Localizer;

    public async Task<bool> SignIn(string? returnUrl = null, string? panelTitle = null, string? panelSubTitle = null)
    {
        signInPanelTcs?.TrySetCanceled();
        signInPanelTcs = new();

        panelTitle ??= Localizer[nameof(AppStrings.Login)]; //or AppStrings.MoreDetailsText
        panelSubTitle ??= Localizer[nameof(AppStrings.LoginToUnlockFullDetails)]; //or AppStrings.JustLogUnlock

        var panelMetadata = new ComponentMetadata
        {
            Type = typeof(SignInModal),
            Parameters = new()
            {
                {
                    nameof(SignInModal.OnClose),
                    new Action(CloseSignInPanel)
                },
                {
                    nameof(SignInModal.OnSuccess),
                    new Action(CloseSignInPanel)
                },
                {
                    nameof(SignInModal.ReturnUrl),
                    returnUrl ?? navigationManager.GetRelativePath()
                },
                {
                    nameof(SignInModal.Title),
                    panelTitle
                },
                {
                    nameof(SignInModal.SubTitle),
                    panelSubTitle
                }
            }
        };

        pubSubService.Publish(ClientPubSubMessages.OPEN_BUTTON_SHIT, panelMetadata);

        return await signInPanelTcs.Task;
    }

    private void CloseSignInPanel()
    {
        if (signInPanelTcs is null)
        {
            return;
        }

        pubSubService.Publish(ClientPubSubMessages.CLOSE_BUTTON_SHIT);
        signInPanelTcs.TrySetResult(false);
    }
    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        //modalReference?.Close();
        signInPanelTcs?.TrySetResult(false);
    }

    public async ValueTask DisposeAsync()
    {
        //modalReference?.Close();
        signInPanelTcs?.TrySetResult(false);
        navigationManager.LocationChanged -= NavigationManager_LocationChanged;
    }
}
