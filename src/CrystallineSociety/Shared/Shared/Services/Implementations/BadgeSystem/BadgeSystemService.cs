using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Services.Implementations.BadgeSystem
{
    public partial class BadgeSystemService : IBadgeSystemService
    {
        [AutoInject]
        public IEnumerable<IBadgeSystemValidator> BadgeValidators { get; set; }

        [AutoInject]
        public ILearnerService LearnerService { get; set; }
        
        public BadgeBundleDto BadgeBundle { get; set; } = new();

        public List<BadgeDto> Badges => BadgeBundle?.Badges ?? throw new Exception("BadgeSystem is not built yet.");
        public List<BadgeSystemValidationDto> Validations => BadgeBundle?.Validations ?? throw new Exception("BadgeSystem is not built yet.");
        public List<BadgeSystemValidationDto> Errors => Validations.Where(v => v.Level == BadgeSystemValidationLevel.Error).ToList();
        public List<BadgeSystemValidationDto> Validate()
        {
            var validations = new List<BadgeSystemValidationDto>();

            if (BadgeBundle is null)
                return validations;

            foreach (var badge in BadgeBundle.Badges)
            {
                foreach (var validator in BadgeValidators)
                {
                    var list = validator.ValidateBadge(badge, BadgeBundle);
                    validations.AddRange(list);
                }
            }

            foreach (var validation in BadgeValidators)
            {
                var list = validation.ValidateBundle(BadgeBundle);
                validations.AddRange(list);
            }

            return validations;
        }

        public BadgeDto GetBadge(string badge)
        {
            return Badges.FirstOrDefault() ?? throw new Exception($"No badge with name: '{badge}'");
        }

        
        public async Task<List<BadgeCountDto>> GetEarnedBadgesAsync(string username)
        {
            var learner = await LearnerService.GetLearnerByUsernameAsync(username);
            var earnedBadgeStrs = learner.GetEarnedBadgeStrs();
            var badges = (
                from earnedBadgeStr in earnedBadgeStrs
                select CreateEarnedBadge(earnedBadgeStr)
            ).ToList();

            return badges;
        }

        public async Task<List<LearnerDto>> GetLearnersHavingBadgeAsync(params BadgeCountDto[] requiredEarnedBadges)
        {
            //var learners = LearnerService.GetLearners()
            //               .Where(l =>
            //               {
            //                   var leanerBadgeCounts = from bc in l.GetEarnedBadgeStrs() select new BadgeCountDto(bc);
            //                   return requiredEarnedBadges.All(r =>
            //                       leanerBadgeCounts.Any(lb =>
            //                           lb.Badge == r.Badge && lb.Count >= r.Count));
            //               })
            //               .ToList();

            var learners = (
                from learner in LearnerService.GetLearners()
                let learnerBadges = learner.GetEarnedBadgeStrs().Select(CreateEarnedBadge)
                where requiredEarnedBadges.All(r => learnerBadges.Any(lb => lb.Badge == r.Badge && lb.Count >= r.Count))
                select learner
            ).ToList();




                //.Where(l =>
                //{
                //    var leanerBadgeCounts = from bc in l.GetEarnedBadgeStrs() select new BadgeCountDto(bc);
                //    return requiredEarnedBadges.All(r =>
                //        leanerBadgeCounts.Any(lb =>
                //            lb.Badge == r.Badge && lb.Count >= r.Count));
                //})
                //.ToList();

            return learners;
        }

        public void Build(BadgeBundleDto bundle)
        {
            BadgeBundle = bundle;

            var validations = Validate();
            bundle.Validations = validations;
        }

        /// <summary>
        /// Example: doc-guru*2
        /// </summary>
        /// <param name="badgeCount">Example: doc-guru*2</param>
        private BadgeCountDto CreateEarnedBadge(string badgeCount)
        {
            var parts = badgeCount.Split('*');
            string badge = parts[0] ?? throw new Exception($"badge is null: '{badgeCount}'");
            
            return new BadgeCountDto
            {
                Badge = GetBadge(badge),
                Count = int.Parse(parts.Length > 1 ? parts[1] : "1")
            };

        }
    }
}
