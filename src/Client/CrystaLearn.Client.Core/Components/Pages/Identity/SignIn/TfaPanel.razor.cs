using CrystaLearn.Shared.Dtos.Identity;

namespace CrystaLearn.Client.Core.Components.Pages.Identity.SignIn;

public partial class TfaPanel
{
    [Parameter] public bool IsWaiting { get; set; }

    [Parameter] public SignInRequestDto Model { get; set; } = default!;

    [Parameter] public EventCallback OnSendTfaToken { get; set; }
    [Parameter] public EventCallback OnTokenProvided { get; set; }
}
