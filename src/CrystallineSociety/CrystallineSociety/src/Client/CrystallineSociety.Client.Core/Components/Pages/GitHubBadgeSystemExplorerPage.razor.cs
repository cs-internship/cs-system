using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components.Pages
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
