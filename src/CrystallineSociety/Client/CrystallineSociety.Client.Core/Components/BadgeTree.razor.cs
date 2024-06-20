using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeTree : ComponentBase
    {
        [Parameter] public BadgeBundleDto? BadgeBundleDto { get; set; }
        [Parameter] public EventCallback<BadgeDto> BadgeDtoCallBack { get; set; }

        private string? ActiveBadgeUrl { get; set; }
        private List<BadgeDto>? Badges { get; set; }
        private Dictionary<int, bool> accordionCollapsed = new Dictionary<int, bool>
        {
            { 0, true },
            { 1, true },
            { 2, true },
            { 3, true }
        };

        private static readonly BadgeLevel[] BadgeLevels = { BadgeLevel.Gold, BadgeLevel.Silver, BadgeLevel.Bronze };

        private void ToggleCollapseAccordion(int itemNumber)
        {
            accordionCollapsed[itemNumber] = !accordionCollapsed[itemNumber];
        }

        private string GetAccordionCollapseClass(int itemNumber)
        {
            return !accordionCollapsed[itemNumber] ? "show" : "";
        }

        protected override void OnParametersSet()
        {
            if (BadgeBundleDto != null)
            {
                Badges = BadgeBundleDto.Badges.ToList();
            }
        }

        private async Task OnBadgeClick(BadgeDto badgeDto)
        {
            ActiveBadgeUrl = badgeDto.Url;
            await BadgeDtoCallBack.InvokeAsync(badgeDto);
        }
    }
}
