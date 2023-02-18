using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.Account;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Contracts
{
    public interface ILeanerService
    {
        Task<LearnerDto> GetLearnerByIdAsync(Guid id);
        Task<LearnerDto> GetLearnerByUsernameAsync(string username);
        IQueryable<Dtos.LearnerDto> GetLearners();
        Task<List<LearnerDto>> GetLearnersHavingBadgeAsync(params BadgeCountDto[] requiredEarnedBadges);
    }
}
