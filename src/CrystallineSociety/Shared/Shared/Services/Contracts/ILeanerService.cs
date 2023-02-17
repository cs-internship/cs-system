using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.Account;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using LearnerDto = CrystallineSociety.Shared.Dtos.Account.LearnerDto;

namespace CrystallineSociety.Shared.Services.Contracts
{
    public interface ILeanerService
    {
        Task<List<BadgeDto>> GetLearnerBadgesAsync(LearnerDto learner);
        Task<LearnerDto> GetLearnerByIdAsync(Guid id);
        Task<LearnerDto> GetLearnerByUsernameAsync(string username);
        IQueryable<Dtos.LearnerDto> GetLearners();

        IQueryable<BadgeDto> GetLearnerBadges(Dtos.LearnerDto learner);
    }
}
