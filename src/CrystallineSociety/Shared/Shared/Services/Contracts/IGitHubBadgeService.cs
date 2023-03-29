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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<List<BadgeDto>> GetBadgesAsync(string url);
        
        /// <summary>
        /// Loads a badge spec from the given <paramref name="url"/> and parses it, and returns a BadgeDto.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Returns the parsed badge.</returns>
        /// <exception cref="FormatException">If the loaded url string is not in a correct format.</exception>
        /// <exception cref="ResourceNotFoundException">If the given url could not be found.</exception>
        Task<BadgeDto> GetBadgeAsync(string url);
    }
}
