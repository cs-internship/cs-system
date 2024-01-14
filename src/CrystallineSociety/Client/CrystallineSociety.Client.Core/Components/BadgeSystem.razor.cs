using System.Text;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeSystem
    {
        [AutoInject] private IBadgeSystemService BadgeSystemService { get; set; } = default!;
        [AutoInject] private IBadgeUtilService BadgeUtilService { get; set; } = default!;
        [Parameter] public BadgeBundleDto? Bundle { get; set; }
        private BadgeDto? BadgeDto {get; set;}


        private string? GetBundleText(BadgeBundleDto bundle)
        {
            var builder = new StringBuilder();
            if (Bundle != null)
            {
                foreach (var badge in Bundle.Badges)
                {
                    builder.AppendLine(BadgeUtilService.SerializeBadge(badge));
                }
            }

            return builder.ToString();
        }

        private void GetBadge(BadgeDto badgeDto)
        {
            BadgeDto = badgeDto;
        }
    }
}
