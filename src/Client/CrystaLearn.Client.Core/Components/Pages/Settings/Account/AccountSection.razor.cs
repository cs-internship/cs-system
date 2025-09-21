using CrystaLearn.Shared.Dtos.Identity;

namespace CrystaLearn.Client.Core.Components.Pages.Settings.Account;

public partial class AccountSection
{
    [CascadingParameter] public UserDto? CurrentUser { get; set; }


    [AutoInject] private IWebAuthnService webAuthnService = default!;


    private bool showPasswordless;


    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();

        if (InPrerenderSession is false)
        {
            showPasswordless = await webAuthnService.IsWebAuthnAvailable();
        }
    }
}
