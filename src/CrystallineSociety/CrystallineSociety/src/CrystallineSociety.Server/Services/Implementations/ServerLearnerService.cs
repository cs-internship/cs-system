using CrystallineSociety.Shared.Dtos.BadgeSystem;
using ILearnerService = CrystallineSociety.Server.Services.Contracts.ILearnerService;

namespace CrystallineSociety.Server.Api.Services.Implementations
{
    internal class ServerLearnerService : ILearnerService
    {
        public Task<List<BadgeDto>> GetLearnerBadgesAsync(LearnerDto learner)
        {
            throw new NotImplementedException();
        }

        public Task<LearnerDto> GetLearnerByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<LearnerDto> GetLearnerByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LearnerDto> GetLearners()
        {
            throw new NotImplementedException();
        }

        public Task<List<LearnerDto>> GetLearnersHavingBadgeAsync(params BadgeCountDto[] requiredEarnedBadges)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BadgeDto> GetLearnerBadges(LearnerDto learner)
        {
            throw new NotImplementedException();
        }
    }
}
