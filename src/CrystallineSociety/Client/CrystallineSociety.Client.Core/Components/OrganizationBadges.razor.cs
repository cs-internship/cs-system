using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class OrganizationBadges
    {
        [Parameter]
        public OrganizationDto Organization { get; set; } = default!;

        [Parameter]
        public BadgeBundleDto? Bundle { get; set; }

        private BadgeDto? BadgeDto { get; set; }

        private void GetBadge(BadgeDto badgeDto)
        {
            BadgeDto = badgeDto;
        }
    }
}
