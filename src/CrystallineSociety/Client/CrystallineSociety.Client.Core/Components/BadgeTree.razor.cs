using CrystallineSociety.Shared.Dtos.BadgeSystem;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeTree : ComponentBase
    {
        [Parameter] public BadgeBundleDto? BadgeBundleDto { get; set; }
        [Parameter] public EventCallback<BadgeDto> BadgeDtoCallBack { get; set; }

        private string? ActiveBadgeUrl { get; set; }
        private List<BadgeDto>? Badges { get; set; }
        private Dictionary<int, bool> accordionCollapsed = new Dictionary<int, bool>();
        private Dictionary<string, bool> subFolderCollapsed = new Dictionary<string, bool>();

        protected override void OnParametersSet()
        {
            if (BadgeBundleDto != null)
            {
                Badges = BadgeBundleDto.Badges.ToList();
            }

            for (int i = 1; i <= 5; i++)
            {
                if (!accordionCollapsed.ContainsKey(i))
                {
                    accordionCollapsed[i] = true;
                }

                for (int j = 1; j <= 3; j++)
                {
                    var key = $"{i}-{j}";
                    if (!subFolderCollapsed.ContainsKey(key))
                    {
                        subFolderCollapsed[key] = true;
                    }
                }
            }
        }

        private void ToggleHomeCollapse(int index)
        {
            if (accordionCollapsed.ContainsKey(index))
            {
                accordionCollapsed[index] = !accordionCollapsed[index];
            }
            else
            {
                // If the key does not exist, add it with a default value
                accordionCollapsed[index] = true;
            }
        }

        private void ToggleSubFolder(int homeIndex, int subIndex)
        {
            var key = $"{homeIndex}-{subIndex}";
            if (subFolderCollapsed.ContainsKey(key))
            {
                subFolderCollapsed[key] = !subFolderCollapsed[key];
            }
            else
            {
                // If the key does not exist, add it with a default value
                subFolderCollapsed[key] = true;
            }
        }

        private async Task OnBadgeClick(BadgeDto badgeDto)
        {
            ActiveBadgeUrl = badgeDto.Url;
            await BadgeDtoCallBack.InvokeAsync(badgeDto);
        }
    }
}
