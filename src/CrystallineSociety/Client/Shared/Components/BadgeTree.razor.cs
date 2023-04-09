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

        private string?[]? BadgeCodes { get; set; }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (BadgeBundleDto != null && firstRender)
            {
                BadgeCodes = BadgeBundleDto.Badges.Select(b => b.Code).ToArray();
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        private void Test()
        {
            if (BadgeBundleDto != null)
            {
                BadgeCodes = BadgeBundleDto.Badges.Select(b => b.Code).ToArray();
            }
        }
    }
}
