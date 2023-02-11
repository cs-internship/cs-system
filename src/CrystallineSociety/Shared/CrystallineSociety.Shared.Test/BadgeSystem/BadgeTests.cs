using CrystallineSociety.Shared.Test.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using CrystallineSociety.Shared.Test.Utils;

namespace CrystallineSociety.Shared.Test.BadgeSystem
{
    [TestClass]
    public class BadgeTests : TestBase
    {
        public TestContext TestContext { get; set; } = default!;

        [TestMethod]
        public async Task Badge_LoadSimple()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var badgeService = testHost.Services.GetService<IBadgeService>();

            Assert.IsNotNull(badgeService);

            var specJson = await ResourceUtil.LoadSampleBadge("serialization-badge-sample");
            var badge = badgeService.ParseBadge(specJson);

            Assert.IsNotNull(badge);
            Assert.IsNotNull(badge.Code);
            Assert.IsNotNull(badge.Description);

            var appraisalMethods = badge.AppraisalMethods!;
            Assert.IsTrue(appraisalMethods.Any());
            
            var firstAppraisal = appraisalMethods.First();
            Assert.IsNotNull(firstAppraisal.Title);
            Assert.IsTrue(firstAppraisal.BadgeRequirements.Any());
            Assert.IsTrue(firstAppraisal.ActivityRequirements.Any());
            Assert.IsTrue(firstAppraisal.ApprovingSteps.Any());

            var firstApprovingStep = firstAppraisal.ApprovingSteps.First();
            Assert.AreEqual(1, firstApprovingStep.Step);
            Assert.AreEqual(2, firstApprovingStep.RequiredApprovalCount);
            Assert.IsNotNull(firstApprovingStep.Title);
            Assert.IsTrue(firstApprovingStep.ApproverRequiredBadges.Any());


            var firstApprovingStepBadge = firstApprovingStep.ApproverRequiredBadges.First();
            Assert.AreEqual(1, firstApprovingStepBadge.RequirementOptions.Count);

            var lastApprovingStepBadge = firstApprovingStep.ApproverRequiredBadges.Last();
            Assert.AreEqual(2, lastApprovingStepBadge.RequirementOptions.Count);

        }

        [TestMethod]
        public async Task Badge_FullRound()
        {
            var testHost = Host.CreateDefaultBuilder()
                               .ConfigureServices((_, services) =>
                                   {
                                       services.AddSharedServices();
                                   }
                               ).Build();

            var badgeService = testHost.Services.GetService<IBadgeService>();

            Assert.IsNotNull(badgeService);

            var specJson = await ResourceUtil.LoadSampleBadge("serialization-badge-sample");
            var badge = badgeService.ParseBadge(specJson);

            var resultJson = badgeService.SerializeBadge(badge);

            Assert.AreEqual(specJson, resultJson);

        }
    }
}