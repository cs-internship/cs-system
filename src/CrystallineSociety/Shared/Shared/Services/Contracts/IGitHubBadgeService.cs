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
        /// <param name="url">The url address that contains badge files or folders containing badge file/folders.</param>
        /// <returns>Returns the parsed badge.</returns>
        /// <exception cref="FormatException">If the loaded url string is not in a correct format.</exception>
        /// <exception cref="ResourceNotFoundException">If the given url could not be found.</exception>
        /// <exception cref="FileNotFoundException">If the given url contains no valid badge file.</exception>
        /// <exception cref="FileContentIsNullException">If the badge file content is null.</exception>
        Task<BadgeDto> GetBadgeAsync(string url);
    }
}
