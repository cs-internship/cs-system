using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Services.Contracts
{
    public interface IGitHubBadgeService
    {
        /// <summary>
        /// Asynchronously retrieves <see cref="BadgeDto"/> objects from badge files located at the GitHub URL provided.
        /// This method performs a recursive search for files and selects those whose filename ends with `-badge.json`.
        /// </summary>
        /// <param name="folderUrl">GitHub folder URL containing badge files.</param>
        /// <returns>A task that represents a list of parsed badge files.</returns>
        /// <exception cref="Exception">When the RepoId of light badge is null.</exception>
        /// <exception cref="Exception">When the Sha of light badge is null.</exception>
        Task<List<BadgeDto>> GetBadgesAsync(string folderUrl);

        /// <summary>
        /// Asynchronously loads a badge specification from the given <paramref name="badgeUrl"/> and parses it.
        /// </summary>
        /// <param name="badgeUrl">The badge file GitHub URL. Badge filename must ends with `-badge.json`.</param>
        /// <returns>A task that represents the parsed badge object.</returns>
        /// <exception cref="ResourceNotFoundException">When unable to locate the GitHub repo branchName.</exception>
        /// <exception cref="FileNotFoundException">When the given <paramref name="badgeUrl"/> is not a valid badge file URL.</exception>
        /// <exception cref="FormatException">When the loaded badge file has an incorrect format and cannot be parsed.</exception>
        Task<BadgeDto> GetBadgeAsync(string badgeUrl);

        /// <summary>
        /// Asynchronously loads and parses a badge specification from a badge file on GitHub identified by the <paramref name="repositoryId"/> and <paramref name="sha"/> parameters.
        /// </summary>
        /// <param name="repositoryId">The Id of the repository on GtiHub.</param>
        /// <param name="sha">The SHA Id of the file in the repository on GtiHub.</param>
        /// <returns>A task that represents the parsed badge object.</returns>
        Task<BadgeDto> GetBadgeAsync(long repositoryId, string sha);
        
        /// <summary>
        /// Asynchronously loads and parses a lightweight version of badges specifications from a GitHub URL pointing to a folder recursively.
        /// </summary>
        /// <param name="folderUrl">The GitHub URL pointing to a folder containing badge file(s). All badge files will load recursively. Badge filename must ends with `-badge.json`.</param>
        /// <returns>A task that represents a list of lightweight version of <see cref="BadgeDto"/>s.</returns>
        Task<List<BadgeDto>> GetLightBadgesAsync(string folderUrl);

        Task<List<ProgramDocumentDto>> GetProgramDocumentsAsync(string folderUrl);
    }
}
