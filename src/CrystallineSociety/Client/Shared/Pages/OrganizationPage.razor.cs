using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

        [Parameter]
        public string? NavTitle { get; set; }

        private OrganizationDto Organization { get; set; } = default!;

        private BadgeBundleDto? Bundle { get; set; }

        private OrganizationNavLink ActiveOrganizationNavLink { get; set; } = OrganizationNavLink.Home;

        protected override async Task OnInitAsync()
        {
            if (NavTitle != null)
            {
                await HandleNavMenuClickAsync(NavTitle);
            }

            else
            {
                await HandleNavMenuClickAsync("Home");
            }

            await LoadOrganizationAsync();

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

        private async Task HandleNavMenuClickAsync(string navTitle)
        {
            ActiveOrganizationNavLink = navTitle.ToLower().Trim() switch
            {
                "home" => OrganizationNavLink.Home,
                "docs" => OrganizationNavLink.Docs,
                "learners" => OrganizationNavLink.Learners,
                "badges" => OrganizationNavLink.Badges,
                "feed" => OrganizationNavLink.Feed,
                _ => throw new NotImplementedException()
            };

            var url = $"/o/{OrganizationCode}/{navTitle.ToLower().Trim()}";

            await JSRuntime.InvokeVoidAsync("App.removeParametersOfUrl", url);

            if (ActiveOrganizationNavLink == OrganizationNavLink.Badges)
            {
                await LoadBadgeSystem();
            }
            else if (ActiveOrganizationNavLink == OrganizationNavLink.Docs)
            {
                await LoadOrganizationAsync();
            }
        }
    }
}
