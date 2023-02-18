using CrystallineSociety.Shared.Test.Infrastructure;
using CrystallineSociety.Shared.Test.Utils;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using CrystallineSociety.Shared.Test.Fake;

namespace CrystallineSociety.Shared.Test.BadgeSystem
{
    [TestClass]
    public class BadgeSystemTests : TestBase
    {
        public TestContext TestContext { get; set; } = default!;

        [TestMethod]
        public async Task Badge_LoadSimple()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                       services.AddTestServices();
                                   }
                               ).Build();

            var badgeService = testHost.Services.GetRequiredService<IBadgeUtilService>();
            var factory = testHost.Services.GetRequiredService<BadgeSystemFactory>();

            var specJson = await ResourceUtil.LoadSampleBadge("serialization-badge-sample");
            var badge = badgeService.ParseBadge(specJson);

            Assert.IsNotNull(badge);

            var bundle = new BadgeBundleDto();
            bundle.Badges.Add(badge);

            var badgeSystem = factory.CreateNew(bundle);

            Assert.IsNotNull(badgeSystem.Validations);
            Assert.IsTrue(badgeSystem.Validations.Any());

        }

        [TestMethod]
        public async Task GetLearnerBadges_Simple()
        {
            var learners = new List<LearnerDto>()
            {
                new("mehran", "doc-guru,doc-master,doc-beginner"),
                new("elaheh", "doc-guru,doc-master*2,doc-beginner"),
                new("zahra_a", "doc-guru,doc-master*2,doc-beginner"),
                new("zahra_r", "doc-master,doc-beginner"),
                new("amin", "doc-master,doc-beginner"),
                new("behzad", "doc-beginner"),
                new("hootan", "doc-beginner"),
            };

            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                       services.AddTestServices();
                                       services.AddSingleton<ILeanerService>(new FakeLearnerService(learners));

                                   }
                               ).Build();

            var badgeUtilService = testHost.Services.GetRequiredService<IBadgeUtilService>();
            var factory = testHost.Services.GetRequiredService<BadgeSystemFactory>();
            var learnerService = testHost.Services.GetRequiredService<ILeanerService>();

            var badgeSpecs = await ResourceUtil.LoadScenarioBadges("scenario-simple-doc");
            var badges = from badgeSpec in badgeSpecs select badgeUtilService.ParseBadge(badgeSpec);

            Assert.IsNotNull(badges);

            var bundle = new BadgeBundleDto();
            bundle.Badges.AddRange(badges);

            var badgeSystem = factory.CreateNew(bundle);

            Assert.IsNotNull(badgeSystem.Validations);
            Assert.IsFalse(badgeSystem.Validations.Any());

            var mehranBadges = await badgeSystem.GetEarnedBadgesAsync("mehran");
            Assert.AreEqual(3, mehranBadges.Count);

            var aminBadges = await badgeSystem.GetEarnedBadgesAsync("amin");
            Assert.AreEqual(2, aminBadges.Count);

            var guruLearners = await learnerService.GetLearnersHavingBadgeAsync(new BadgeCountDto("doc-guru"));
            Assert.AreEqual(3, guruLearners.Count);

            var masterLearners = await learnerService.GetLearnersHavingBadgeAsync(new BadgeCountDto("doc-master"));
            Assert.AreEqual(5, masterLearners.Count);

            var master2Learners = await learnerService.GetLearnersHavingBadgeAsync(new BadgeCountDto("doc-master*2"));
            Assert.AreEqual(2, master2Learners.Count);

            var beginnerLearners = await learnerService.GetLearnersHavingBadgeAsync(new BadgeCountDto("doc-beginner"));
            Assert.AreEqual(7, beginnerLearners.Count);

        }
    }
}