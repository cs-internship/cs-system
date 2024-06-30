using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeTree : ComponentBase
    {
        [Parameter] public BadgeBundleDto? BadgeBundleDto { get; set; }
        [Parameter] public EventCallback<BadgeDto> BadgeDtoCallBack { get; set; }

        private string? ActiveBadgeUrl { get; set; }
        private List<BadgeDto>? Badges { get; set; }
        private Dictionary<int, bool> accordionCollapsed = new Dictionary<int, bool>();
        private static readonly BadgeLevel[] BadgeLevels = { BadgeLevel.Gold, BadgeLevel.Silver, BadgeLevel.Bronze };

        protected override void OnParametersSet()
        {
            if (BadgeBundleDto != null)
            {
                Badges = BadgeBundleDto.Badges.ToList();
            }

            for (int i = 0; i < BadgeLevels.Length; i++)
            {
                if (!accordionCollapsed.ContainsKey(i + 1))
                {
                    accordionCollapsed[i + 1] = true;
                }
            }
        }

        private async Task OnBadgeClick(BadgeDto badgeDto)
        {
            ActiveBadgeUrl = badgeDto.Url;
            await BadgeDtoCallBack.InvokeAsync(badgeDto);
        }
    }
}
