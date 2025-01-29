﻿using System.Reflection;
using CrystaLearn.Shared.Dtos.PushNotification;

namespace Microsoft.JSInterop;

public static partial class IJSRuntimeExtensions
{
    public static ValueTask<string> GetTimeZone(this IJSRuntime jsRuntime)
    {
        return jsRuntime.InvokeAsync<string>("App.getTimeZone");
    }


    public static async ValueTask<PushNotificationSubscriptionDto> GetPushNotificationSubscription(this IJSRuntime jsRuntime, string vapidPublicKey)
    {
        return await jsRuntime.InvokeAsync<PushNotificationSubscriptionDto>("App.getPushNotificationSubscription", vapidPublicKey);
    }

    /// <summary>
    /// The return value would be false during pre-rendering
    /// </summary>
    public static bool IsInitialized(this IJSRuntime jsRuntime)
    {
        if (jsRuntime is null)
            return false;

        var type = jsRuntime.GetType();

        return type.Name switch
        {
            "UnsupportedJavaScriptRuntime" => false, // pre-rendering
            "RemoteJSRuntime" /* blazor server */ => (bool)type.GetProperty("IsInitialized")!.GetValue(jsRuntime)!,
            "WebViewJSRuntime" /* blazor hybrid */ => type.GetField("_ipcSender", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(jsRuntime) is not null,
            _ => true // blazor wasm
        };
    }
}
