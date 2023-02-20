using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.TodoItem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Client.Shared.Pages;

public partial class DefaultBadgeSystemPage
{
    [AutoInject]
    public required BadgeSystemFactory BadgeSystemFactory { get; set; }

    private BadgeBundleDto DefaultBundle { get; set; } = new();

    protected override async Task OnInitAsync()
    {
        await LoadDefaultBundleAsync();
    }

    private async Task LoadDefaultBundleAsync()
    {
        var badgeSystem = BadgeSystemFactory.Default();
        await Task.Delay(TimeSpan.FromSeconds(3));
        DefaultBundle = new BadgeBundleDto() { Badges = new List<BadgeDto>() { new BadgeDto() { Code = "hello" } } };
        StateHasChanged();
    }
}
