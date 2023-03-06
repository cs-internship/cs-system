using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;


namespace CrystallineSociety.Client.Shared.Components
{
    public partial class BadgeSystem
    {
        [AutoInject] private IBadgeSystemService BadgeSystemService { get; set; } = default!;
        [AutoInject] private IBadgeUtilService BadgeUtilService { get; set; } = default!;
        [Parameter] public BadgeBundleDto? Bundle { get; set; }

        private string? BundleText { get; set; }

        protected override Task OnInitializedAsync()
        {
            RefreshBundle();
            return base.OnInitializedAsync();
        }

        private void RefreshBundle()
        {
            var builder = new StringBuilder();
            if (Bundle != null)
            {
                foreach (var badge in Bundle.Badges)
                {
                    builder.AppendLine(BadgeUtilService.SerializeBadge(badge));
                }
            }

            BundleText = builder.ToString();

        }
    }
}
