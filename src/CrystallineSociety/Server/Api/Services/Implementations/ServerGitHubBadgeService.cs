using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Api.Services.Implementations
{
    public class ServerGitHubBadgeService : IGitHubBadgeService
    {
        public Task<List<BadgeDto>> GetBadgesAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDto> GetBadgeAsync(string url)
        {
            throw new NotImplementedException();
        }
    }
}
