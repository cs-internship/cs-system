using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.TodoItem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Client.Shared.Pages;

public partial class DefaultBadgeSystemPage
{
    [AutoInject]
    public required BadgeSystemFactory BadgeSystemFactory { get; set; }

    private BadgeBundleDto? DefaultBundle { get; set; } = new();

    protected override async Task OnInitAsync()
    {
        await LoadDefaultBundleAsync();
    }

    private async Task LoadDefaultBundleAsync()
    {
        DefaultBundle = await HttpClient.GetFromJsonAsync("BadgeSystem/GetDefaultBadgeBundle", AppJsonContext.Default.BadgeBundleDto);
        StateHasChanged();
    }
}
