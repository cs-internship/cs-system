namespace CrystaLearn.Client.Core.Components.Pages.Identity.SignIn;

public partial class SignInModal
{
    [Parameter] public string? ReturnUrl { get; set; }
    [Parameter] public Action? OnClose { get; set; }
    [Parameter] public Action? OnSuccess { get; set; } // The SignInModalService will show this page as a modal dialog, and this action will be invoked when the sign-in is successful.

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? SubTitle { get; set; }
    [Parameter] public bool IsFromChat { get; set; } = false;

    private void CloseModal()
    {
        OnClose?.Invoke();
    }
}
