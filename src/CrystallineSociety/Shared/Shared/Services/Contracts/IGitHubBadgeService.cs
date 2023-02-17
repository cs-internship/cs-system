using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Contracts
{
    public interface IGitHubBadgeService
    {
        Task<List<BadgeDto>> GetBadgesAsync(string url);
        Task<BadgeDto> GetBadgeAsync(string url);
    }
}
