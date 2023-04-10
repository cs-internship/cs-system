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

        private List<BadgeDto>? BadgeCodes { get; set; }

        protected override Task OnParametersSetAsync()
        {
            if (BadgeBundleDto != null)
            {
                BadgeCodes = BadgeBundleDto.Badges.ToList();
            }
            return base.OnParametersSetAsync();
        }
    }
}
