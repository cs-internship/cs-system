using CrystaLearn.Shared.Dtos.PushNotification;

namespace CrystaLearn.Client.Windows.Services;

public partial class WindowsPushNotificationService : PushNotificationServiceBase
{
    public override Task<PushNotificationSubscriptionDto> GetSubscription(CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
