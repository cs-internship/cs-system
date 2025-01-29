﻿using CrystaLearn.Shared.Dtos.Identity;
using CrystaLearn.Server.Api.Models.PushNotification;

namespace CrystaLearn.Server.Api.Models.Identity;

public partial class UserSession
{
    public Guid Id { get; set; }

    public string? IP { get; set; }

    /// <summary>
    /// <inheritdoc cref="UserSessionDto.DeviceInfo"/>
    /// </summary>
    public string? DeviceInfo { get; set; }

    public string? Address { get; set; }

    /// <summary>
    /// <inheritdoc cref="AuthPolicies.PRIVILEGED_ACCESS"/>
    /// </summary>
    public bool Privileged { get; set; }

    public DateTimeOffset StartedOn { get; set; }

    public DateTimeOffset? RenewedOn { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public PushNotificationSubscription? PushNotificationSubscription { get; set; }

    public string? SignalRConnectionId { get; set; }
}
