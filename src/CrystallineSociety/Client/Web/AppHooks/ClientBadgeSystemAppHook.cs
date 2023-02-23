using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Client.Web.AppHooks
{
    //public partial class ClientBadgeSystemAppHook : IAppHook
    //{
    //    [AutoInject] 
    //    public IHttpClientFactory HttpClientFactory { get; set; } = default!;
    //    [AutoInject]
    //    public BadgeSystemFactory Factory { get; set; }
    //    public async Task OnStartup()
    //    {
    //        var client = HttpClientFactory.CreateClient();
    //        var bundle = await client.GetFromJsonAsync<BadgeBundleDto>("api/BadgeSystem/GetDefaultBadgeBundle");
    //        if (bundle != null)
    //            Factory.SetCurrentBadgeSystem(bundle);

    //    }
    //}
}
