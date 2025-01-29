using CrystaLearn.Shared.Dtos.PushNotification;

namespace CrystaLearn.Client.Core.Services.Contracts;

public interface IPushNotificationService
{
    string Token { get; set; }
    Task<bool> IsPushNotificationSupported(CancellationToken cancellationToken);
    Task<PushNotificationSubscriptionDto> GetSubscription(CancellationToken cancellationToken);
    Task Subscribe(CancellationToken cancellationToken);
}
