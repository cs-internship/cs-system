using CrystaLearn.Core.Models.PushNotification;
using CrystaLearn.Shared.Dtos.PushNotification;
using Riok.Mapperly.Abstractions;

namespace CrystaLearn.Server.Api.Mappers;

/// <summary>
/// More info at Server/Mappers/README.md
/// </summary>
[Mapper]
public static partial class PushNotificationMapper
{
    public static partial void Patch(this PushNotificationSubscriptionDto source, PushNotificationSubscription destination);
}
