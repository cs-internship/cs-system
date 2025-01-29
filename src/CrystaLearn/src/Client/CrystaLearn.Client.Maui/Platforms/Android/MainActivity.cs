using Android.OS;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Java.Net;
using Android.Gms.Tasks;
using Firebase.Messaging;
using CrystaLearn.Client.Core.Components;

namespace CrystaLearn.Client.Maui.Platforms.Android;

[IntentFilter([Intent.ActionView],
                        DataSchemes = ["https", "http"],
                        DataHosts = ["use-your-web-app-url-here.com"],
                        // the following app links will be opened in app instead of browser if the app is installed on Android device.
                        DataPaths = [Urls.HomePage],
                        DataPathPrefixes = [
                            "/en-US", "/en-GB", "/fa-IR", "/nl-NL",
                            Urls.ConfirmPage, Urls.ForgotPasswordPage, Urls.SettingsPage, Urls.ResetPasswordPage, Urls.SignInPage, Urls.SignUpPage, Urls.NotAuthorizedPage, Urls.NotFoundPage, Urls.TermsPage, Urls.AboutPage,
                            ],
                        AutoVerify = true,
                        Categories = [Intent.ActionView, Intent.CategoryDefault, Intent.CategoryBrowsable])]

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public partial class MainActivity : MauiAppCompatActivity
    , IOnSuccessListener
{
    private IPushNotificationService PushNotificationService => IPlatformApplication.Current!.Services.GetRequiredService<IPushNotificationService>();

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        // https://github.com/dotnet/maui/issues/24742
        Theme?.ApplyStyle(Resource.Style.OptOutEdgeToEdgeEnforcement, force: false);

        base.OnCreate(savedInstanceState);

        var url = Intent?.DataString;
        if (string.IsNullOrWhiteSpace(url) is false)
        {
            _ = Routes.OpenUniversalLink(new URL(url).File ?? Urls.HomePage);
        }
        PushNotificationService.IsPushNotificationSupported(default).ContinueWith(task =>
        {
            if (task.Result)
            {
                FirebaseMessaging.Instance.GetToken().AddOnSuccessListener(this);
            }
        });
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);

        var action = intent!.Action;
        var url = intent.DataString;
        if (action is Intent.ActionView && string.IsNullOrWhiteSpace(url) is false)
        {
            _ = Routes.OpenUniversalLink(new URL(url).File ?? Urls.HomePage);
        }
    }

    public void OnSuccess(Java.Lang.Object? result)
    {
        PushNotificationService.Token = result!.ToString();
    }
}
