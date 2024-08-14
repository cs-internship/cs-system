using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeTree : ComponentBase
    {
        [Parameter] public BadgeBundleDto? BadgeBundleDto { get; set; }
        [Parameter] public EventCallback<BadgeDto> BadgeDtoCallBack { get; set; }

        private Dictionary<int, bool> homeCollapseState = new();
        private Dictionary<(int, int, int), bool> subFolderCollapseState = new();
        private List<BadgeDto>? Badges { get; set; }
        private string? ActiveBadgeUrl { get; set; }


        protected override void OnParametersSet()
        {
            if (BadgeBundleDto != null)
            {
                Badges = BadgeBundleDto.Badges.ToList();
            }
        }

        private void ToggleHomeCollapse(int index)
        {
            if (homeCollapseState.ContainsKey(index))
            {
                homeCollapseState[index] = !homeCollapseState[index];
            }
            else
            {
                homeCollapseState[index] = true;
            }
        }

        private void ToggleSubFolder(int folderIndex, int badgeIndex, int subFolderIndex)
        {
            var key = (folderIndex, badgeIndex, subFolderIndex);
            if (subFolderCollapseState.ContainsKey(key))
            {
                subFolderCollapseState[key] = !subFolderCollapseState[key];
            }
            else
            {
                subFolderCollapseState[key] = true;
            }
        }

        private async Task OnBadgeClick(BadgeDto badgeDto)
        {
            ActiveBadgeUrl = badgeDto.Url;
            await BadgeDtoCallBack.InvokeAsync(badgeDto);
        }

        private string GetArrowClass(bool isExpanded) => isExpanded ? "down-arrow" : "right-arrow";
    }
}
