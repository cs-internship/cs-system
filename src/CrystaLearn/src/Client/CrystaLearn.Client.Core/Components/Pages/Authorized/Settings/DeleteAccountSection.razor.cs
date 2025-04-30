using CrystaLearn.Shared.Controllers.Identity;

namespace CrystaLearn.Client.Core.Components.Pages.Authorized.Settings;

public partial class DeleteAccountSection
{
    private bool isDialogOpen;

    [AutoInject] IUserController userController = default!;

    private async Task DeleteAccount()
    {
        if (await AuthManager.TryEnterElevatedAccessMode(CurrentCancellationToken))
        {
            await userController.Delete(CurrentCancellationToken);

            await AuthManager.SignOut(CurrentCancellationToken);
        }
    }
}
