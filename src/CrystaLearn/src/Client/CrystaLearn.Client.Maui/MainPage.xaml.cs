
namespace CrystaLearn.Client.Maui;

public partial class MainPage
{
    public MainPage(ClientMauiSettings clientMauiSettings)
    {
        InitializeComponent();
        AppWebView.RootComponents.Insert(0, new()
        {
            ComponentType = typeof(BlazorApplicationInsights.ApplicationInsightsInit),
            Selector = "head::after"
        });
    }
}
