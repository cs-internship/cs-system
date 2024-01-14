using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components.Pages;

public partial class DefaultBadgeSystemPage
{
    [AutoInject]
    public required BadgeSystemFactory BadgeSystemFactory { get; set; }

    private BadgeBundleDto? DefaultBundle { get; set; } = new();

    protected override async Task OnInitAsync()
    {
        await PrerenderStateService.GetValue(async() => LoadDefaultBundleAsync());
    }

    private async Task LoadDefaultBundleAsync()
    {
        DefaultBundle = await HttpClient.GetFromJsonAsync("api/BadgeSystem/GetDefaultBadgeBundle", AppJsonContext.Default.BadgeBundleDto);
        StateHasChanged();
    }
}
