using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;


namespace CrystallineSociety.Client.Shared.Pages
{
    public partial class GitHubBadgeSystemExplorerPage
    {
        private string? GitHubUrl { get; set; }
        private BadgeBundleDto? Bundle { get; set; }

        public GitHubBadgeSystemExplorerPage()
        {

        }

        private async Task LoadBadgeSystem()
        {
            Bundle = await HttpClient.GetFromJsonAsync($"BadgeSystem/GetBadgeBundleFromGitHub?url={GitHubUrl}", AppJsonContext.Default.BadgeBundleDto);
        }
    }
}
