using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using IAppHook = CrystallineSociety.Server.Services.Contracts.IAppHook;
using IBadgeUtilService = CrystallineSociety.Server.Services.Contracts.IBadgeUtilService;

namespace CrystallineSociety.Server.Api.AppHooks
{
    public partial class ServerBadgeSystemAppHook : IAppHook
    {
        [AutoInject]
        public BadgeSystemFactory Factory { get; set; }

        [AutoInject] private IBadgeUtilService BadgeUtil { get; set; }

        public async Task OnStartup()
        {
            var badge1 = """
                                {
                  "code": "doc-guru-badge-sample",
                  "description": "Description for doc-guru-badge-sample",
                  "level": "Gold",
                  "info": {
                    "en": "info_en.md",
                    "fa": "info_fa.md"
                  },
                  "appraisal-methods": [
                    {
                      "title": "Main Method",
                      "badge-requirements": [
                        "requirement-badge-code-A*2",
                        "requirement-badge-code-B",
                        "requirement-badge-code-C*1|requirement-badge-code-D*2"
                      ],
                      "activity-requirements": [
                        "requirement-activity-code-A",
                        "requirement-activity-code-B*1",
                        "requirement-activity-code-C|requirement-activity-code-D*2"
                      ],
                      "approving-steps": [
                        {
                          "step": 1,
                          "title": "Initial Approval",
                          "approver-required-badges": [
                            "requirement-badge-code-A*2",
                            "requirement-badge-code-B",
                            "requirement-badge-code-C*2|requirement-badge-code-D"
                          ],
                          "required-approval-count": 2
                        },
                        {
                          "step": 2,
                          "title": "Final Approval",
                          "approver-required-badges": [
                            "requirement-badge-code-D*2"
                          ],
                          "required-approval-count": 2
                        }
                      ]
                    },
                    {
                      "title": "Superpower Method",
                      "badge-requirements": [],
                      "activity-requirements": [],
                      "approving-steps": [
                        {
                          "step": 1,
                          "title": "Superpower Admin",
                          "approver-required-badges": [
                            "superpower"
                          ],
                          "required-approval-count": 1
                        }
                      ]
                    }
                  ]
                }
                """;


            var bundle = new BadgeBundleDto()
            {
                Badges = new List<BadgeDto>
                {
                    BadgeUtil.ParseBadge(badge1)
                }
            };

            Factory.SetCurrentBadgeSystem(bundle);
        }
    }
}
