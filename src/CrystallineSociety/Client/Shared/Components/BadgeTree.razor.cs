using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Shared.Components
{
    public partial class BadgeTree
    {
        [Parameter] public BadgeBundleDto? BadgeBundleDto { get; set; }
        [Parameter] public EventCallback<BadgeDto> BadgeDtoCallBack { get; set; }

        private string? ActiveBadgeUrl { get; set; }

        private List<BadgeDto>? Badges { get; set; }

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
