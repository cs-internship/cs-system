namespace CrystaLearn.Client.Core.Components.Pages.Identity.Components;

public partial class SocialRow
{
    [Parameter] public EventCallback<string> OnClick { get; set; }

    [Parameter] public bool IsWaiting { get; set; }

    private async Task HandleGoogle()
    {
       
    }

    private async Task HandleFacebook()
    {
    }

    private async Task HandleMicrosoft()
    {
        await OnClick.InvokeAsync("Microsoft");
    }

    private async Task HandleApple()
    {
    }
}
