using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Shared.Pages
{
    public partial class OrganizationPage
    {
        [Parameter]
        public string? OrganizationCode { get; set; }

        private OrganizationDto Organization { get; set; } = default!;

        private BadgeBundleDto? Bundle { get; set; }

        protected override async Task OnInitAsync()
        {
            await LoadOrganizationAsync();
            await LoadBadgeSystem();
            await base.OnInitAsync();
        }

        private async Task LoadOrganizationAsync()
        {
            Organization = await HttpClient.GetFromJsonAsync($"Organization/GetOrganizationByCode?code={OrganizationCode}", AppJsonContext.Default.OrganizationDto);
        }

        private async Task LoadBadgeSystem()
        {
            Bundle = await HttpClient.GetFromJsonAsync($"BadgeSystem/GetBadgeBundleFromGitHub?url={Organization.BadgeSystemUrl}", AppJsonContext.Default.BadgeBundleDto);
        }
    }
}
