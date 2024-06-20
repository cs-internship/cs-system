using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeTree
    {
        [Parameter] public BadgeBundleDto? BadgeBundleDto { get; set; }
        [Parameter] public EventCallback<BadgeDto> BadgeDtoCallBack { get; set; }

        private string? ActiveBadgeUrl { get; set; }

        private List<BadgeDto>? Badges { get; set; }
        private Dictionary<int, bool> accordionCollapsed = new Dictionary<int, bool>
        {
            { 1, true },
            { 2, true },
            { 3, true }
        };

        private void ToggleCollapseAccordion(int itemNumber)
        {
            accordionCollapsed[itemNumber] = !accordionCollapsed[itemNumber];
        }

        private string GetAccordionCollapseClass(int itemNumber)
        {
            return !accordionCollapsed[itemNumber] ? "show" : "";
        }

        protected override Task OnParamsSetAsync()
        {
            if (BadgeBundleDto != null)
            {
                Badges = BadgeBundleDto.Badges.ToList();
            }
            return base.OnParamsSetAsync();
        }

        private async Task OnBadgeClick(BadgeDto badgeDto)
        {
            ActiveBadgeUrl = badgeDto.Url;
            await BadgeDtoCallBack.InvokeAsync(badgeDto);
        }
    }
}
