using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;

namespace CrystallineSociety.Shared.Test.Fake
{
    public partial class FakeLearnerService : ILeanerService
    {
        private List<LearnerDto> Learners { get; set; }

        public FakeLearnerService(List<LearnerDto> learners)
        {
            Learners = learners;
        }

        public async Task<LearnerDto> GetLearnerByIdAsync(Guid id)
        {
            return Learners.First(l => l.Id == id);
        }

        public async Task<LearnerDto> GetLearnerByUsernameAsync(string username)
        {
            return Learners.First(l => l.Username == username);
        }

        public IQueryable<LearnerDto> GetLearners()
        {
            return Learners.AsQueryable();
        }
    }
}
