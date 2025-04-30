using CrystaLearn.Server.Api.Services;
using CrystaLearn.Shared.Dtos.PushNotification;
using CrystaLearn.Shared.Controllers.PushNotification;

namespace CrystaLearn.Server.Api.Controllers.PushNotification;

[Route("api/[controller]/[action]")]
[ApiController, AllowAnonymous]
public partial class PushNotificationController : AppControllerBase, IPushNotificationController
{
    [AutoInject] PushNotificationService pushNotificationService = default!;

    [HttpPost]
    public async Task Subscribe([Required] PushNotificationSubscriptionDto subscription, CancellationToken cancellationToken)
    {
        await pushNotificationService.Subscribe(subscription, cancellationToken);
    }

    [HttpPost("{deviceId}")]
    public async Task Unsubscribe([Required] string deviceId, CancellationToken cancellationToken)
    {
        await pushNotificationService.Unsubscribe(deviceId, cancellationToken);
    }
}
