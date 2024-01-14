using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Shared.Services.Contracts
{
    public interface ILearnerService
    {
        Task<LearnerDto> GetLearnerByIdAsync(Guid id);
        Task<LearnerDto> GetLearnerByUsernameAsync(string username);
        IQueryable<Dtos.LearnerDto> GetLearners();
    }
}
