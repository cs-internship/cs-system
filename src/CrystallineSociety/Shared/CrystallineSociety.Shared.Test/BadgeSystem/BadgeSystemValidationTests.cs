using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Services.Implementations.BadgeSystem;
using CrystallineSociety.Shared.Test.Fake;
using CrystallineSociety.Shared.Test.Infrastructure;
using CrystallineSociety.Shared.Test.Utils;
using Microsoft.Extensions.Hosting;

namespace CrystallineSociety.Shared.Test.BadgeSystem;

[TestClass]
public class BadgeSystemValidationTests : TestBase
{
    public TestContext TestContext { get; set; } = default!;

    [TestMethod]
    public void RepeatedAppraisalMethodValidator_NoErrors()
    {
        var badge1 = """
                    {
                      "code": "doc-guru",
                      "description": "Description for doc-guru",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "activity-requirements": [
                            "requirement-activity-code-A",
                            "requirement-activity-code-B:1",
                            "requirement-activity-code-C|requirement-activity-code-D:2"
                          ],

                        "approving-steps": [
                            {
                              "step": 1,
                              "title": "First Approval",
                              "approver-required-badges": [
                                "doc-guru"
                              ],
                              "required-approval-count": 4
                            },
                            {
                              "step": 2,
                              "title": "Last Approval",
                              "approver-required-badges": [
                                "doc-guru"
                              ],
                              "required-approval-count": 3 
                            }
                          ]
                        }
                      ]
                    }      
                    """;
        var badgeSystem = CreateBadgeSystem(new[] { badge1 });

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            0,
            badgeSystem.Errors.Count());
    }

    [TestMethod]
    public void
       RepeatedAppraisalMethodValidator_BadgeBundleContainingSingleInvalidBadge_ListOfErrorsForTheBadgeWithRepeatedTitlesInTheirAppraisalMethod()
    {
        var badge1 = """
                    {
                      "code": "doc-guru",
                      "description": "Description for doc-guru",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "activity-requirements": [
                            "requirement-activity-code-A",
                            "requirement-activity-code-B:1",
                            "requirement-activity-code-C|requirement-activity-code-D:2"
                          ]
                          
                        },
                        {
                          "title": "Main Method",
                          "activity-requirements": [
                            "requirement-activity-code-A",
                            "requirement-activity-code-B:1",
                            "requirement-activity-code-C|requirement-activity-code-D:2"
                          ]
                          
                        }
                      ]
                   
                    }      
                    """;


        var badgeSystem = CreateBadgeSystem(new[] { badge1 });

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(e =>
                e.Title.Equals(
                    "Repeated appraisal method: Main Method")));

        Assert.AreEqual("Main Method", badgeSystem.Errors.Find(t => t.Title.Equals("Repeated appraisal method: Main Method")).RefBadge);

        Assert.AreEqual(
            "Main Method", badgeSystem.Errors.Find(t =>
                    t.Title.Equals(
                        "Repeated appraisal method: Main Method"))
                .RefBadge);
    }

    [TestMethod]
    public void
       RepeatedAppraisalMethodValidator_BadgeBundleContainingMultipleInvalidBadge_ListOfErrorsForTheBadgeWithRepeatedTitlesInTheirAppraisalMethod()
    {
        var badge1 = """
                    {
                      "code": "doc-guru",
                      "description": "Description for doc-guru",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "activity-requirements": [
                            "requirement-activity-code-A",
                            "requirement-activity-code-B:1",
                            "requirement-activity-code-C|requirement-activity-code-D:2"
                          ]
                          
                        },
                        {
                          "title": "Main Method",
                          "activity-requirements": [
                            "requirement-activity-code-A",
                            "requirement-activity-code-B:1",
                            "requirement-activity-code-C|requirement-activity-code-D:2"
                          ]
                          
                        }
                      ]
                   
                    }      
                    """;

        var badge2 = """
                    {
                      "code": "doc-guru",
                      "description": "Description for doc-guru",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "activity-requirements": [
                            "requirement-activity-code-A",
                            "requirement-activity-code-B:1",
                            "requirement-activity-code-C|requirement-activity-code-D:2"
                          ]
                          
                        },
                        {
                          "title": "Main Method",
                          "activity-requirements": [
                            "requirement-activity-code-A",
                            "requirement-activity-code-B:1",
                            "requirement-activity-code-C|requirement-activity-code-D:2"
                          ]
                          
                        }
                      ]
                   
                    }      
                    """;

        var badgeSystem = CreateBadgeSystem(new[] { badge1, badge2 });

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            2,
            badgeSystem.Errors.Count(e =>
                e.Title.Equals(
                    "Repeated appraisal method: Main Method")));

        Assert.AreEqual("Main Method", badgeSystem.Errors.Find(t => t.Title.Equals("Repeated appraisal method: Main Method")).RefBadge);

        Assert.AreEqual(
            "Main Method", badgeSystem.Errors.Find(t =>
                    t.Title.Equals(
                        "Repeated appraisal method: Main Method"))
                .RefBadge);
    }
    [TestMethod]
    public void RepeatedApprovingStepsValidator_BadgeBundleContainingSingleValidBadge_NoErrors()
    {
        var badge1 = """
                    {
                      "code": "doc-guru",
                      "description": "Description for doc-guru",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "approving-steps": [
                            {
                              "step": 1,
                              "title": "Initial Approval",
                              "approver-required-badges": [
                                "doc-guru"
                              ],
                              "required-approval-count": 4
                            }
                          ]
                        }
                      ]
                    }      
                    """;

        var badgeSystem = CreateBadgeSystem(new[] {badge1});

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            0,
            badgeSystem.Errors.Count());
    }

    [TestMethod]
    public void
        RepeatedApprovingStepsValidator_BadgeBundleContainingSingleInvalidBadge_ListOfErrorsForTheBadgeWithRepeatedTitlesInTheirApprovingSteps()
    {
        var badge1 = """
                    {
                      "code": "doc-guru",
                      "description": "Description for doc-guru",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "approving-steps": [
                            {
                              "step": 1,
                              "title": "Initial Approval",
                              "approver-required-badges": [
                                "doc-guru"
                              ],
                              "required-approval-count": 4
                            },
                            {
                              "step": 2,
                              "title": "Initial Approval",
                              "approver-required-badges": [
                                "doc-master"
                              ],
                              "required-approval-count": 3 
                            }
                          ]
                        }
                      ]
                    }      
                    """;

        var badgeSystem = CreateBadgeSystem(new[] {badge1});

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(e =>
                e.Title.Equals(
                    "Repeated titles in approving steps of doc-guru badge found. Approving steps are not unique.")));
        Assert.AreEqual(
            "doc-guru", badgeSystem.Errors.Find(t =>
                    t.Title.Equals(
                        "Repeated titles in approving steps of doc-guru badge found. Approving steps are not unique."))
                .RefBadge);
    }

    [TestMethod]
    public void
        RepeatedApprovingStepsValidator_BadgeBundleContainingMultipleInvalidBadge_ListOfErrorsForBadgesWithRepeatedTitlesInTheirApprovingSteps()
    {
        var badge1 = """
                    {
                      "code": "doc-guru",
                      "description": "Description for doc-guru",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "approving-steps": [
                            {
                              "step": 1,
                              "title": "Initial Approval",
                              "approver-required-badges": [
                                "doc-guru"
                              ],
                              "required-approval-count": 4
                            },
                            {
                              "step": 2,
                              "title": "Initial Approval",
                              "approver-required-badges": [
                                "doc-master"
                              ],
                              "required-approval-count": 3 
                            }
                          ]
                        }
                      ]
                    }      
                    """;

        var badge2 = """
                    {
                      "code": "doc-beginner",
                      "description": "Description for doc-beginner",
                      "level": "Gold",
                      "appraisal-methods": [
                        {
                          "title": "Main Method",
                          "approving-steps": [
                            {
                              "step": 1,
                              "title": "Last Approval",
                              "approver-required-badges": [
                                "doc-guru"
                              ],
                              "required-approval-count": 4
                            },
                            {
                              "step": 2,
                              "title": "Last Approval",
                              "approver-required-badges": [
                                "doc-guru"
                              ],
                              "required-approval-count": 3 
                            }
                          ]
                        }
                      ]
                    }      
                    """;

        var badgeSystem = CreateBadgeSystem(new[] {badge1, badge2});

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(e =>
                e.Title.Equals(
                    "Repeated titles in approving steps of doc-guru badge found. Approving steps are not unique.")));
        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(e =>
                e.Title.Equals(
                    "Repeated titles in approving steps of doc-beginner badge found. Approving steps are not unique.")));

        Assert.AreEqual(
            "doc-guru", badgeSystem.Errors.Find(t =>
                    t.Title.Equals(
                        "Repeated titles in approving steps of doc-guru badge found. Approving steps are not unique."))
                .RefBadge);
        Assert.AreEqual(
            "doc-beginner", badgeSystem.Errors.Find(t =>
                    t.Title.Equals(
                        "Repeated titles in approving steps of doc-beginner badge found. Approving steps are not unique."))
                .RefBadge);
    }

    [TestMethod]
    public void RequirementsHaveBadgesValidation_MustWork()
    {
        var badge1 =
            """
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
            "requirement-badge-code-B"
                ],
            "approving-steps": [
            {
                "step": 1,
                "title": "Initial Approval",
                "approver-required-badges": [
                "requirement-badge-code-A*2",
                "requirement-badge-code-C*2|requirement-badge-code-D"
                    ],
                "required-approval-count": 2
            }
            ]
        }
        ]
    }
    """;

        var badge2 =
            """
               {
                    "code": "requirement-badge-code-A"
                }
            """;

        var badgeSystem = CreateBadgeSystem(new[] {badge1, badge2});

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.IsTrue(badgeSystem.Errors.Any(v => v.Title.Contains("requirement-badge-code-B")));
        Assert.IsTrue(badgeSystem.Errors.Any(v => v.Title.Contains("requirement-badge-code-C")));
        Assert.IsTrue(badgeSystem.Errors.Any(v => v.Title.Contains("requirement-badge-code-D")));
        Assert.IsTrue(badgeSystem.Errors.Any(v => !v.Title.Contains("requirement-badge-code-A")));
    }

    [TestMethod]
    public void BadgeNameValidation_MustWork()
    {
        var specJson = new List<string>()
        {
            """{"code": "doc-beginner"}""",
            """{"code": "doc-*guru"}""",
            """{"code": "doc-/guru"}""",
            """{"code": "doc-+guru"}""",
            """{"code": "doc-?guru"}""",
            """{"code": "doc-|guru"}""",
            """{"code": "doc-!guru"}""",
            """{"code": "doc-,guru"}""",
            """{"code": "doc-^guru"}""",
        };

        var badgeSystem = CreateBadgeSystem(specJson);

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(8, badgeSystem.Errors.Count(v => v.Title.Contains("Invalid badge name")));
    }

    [TestMethod]
    public void CircularDependencyValidation_MustWork()
    {
        var specJsons = new List<string>()
        {
            """
            {
              "code": "badge-a",
              "appraisal-methods": [
                {
                  "title": "Main Method",
                  "badge-requirements": [
                    "badge-b"
                  ]
                }
              ]
            }
            """,
            """
            {
              "code": "badge-b",
              "appraisal-methods": [
                {
                  "title": "Main Method",
                  "badge-requirements": [
                    "badge-a"
                  ]
                }
              ]
            }
            """,
        };

        var badgeSystem = CreateBadgeSystem(specJsons);

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(v =>
                v.Title.Contains("Circular")
                && v.Title.Contains("badge-a")
                && v.Title.Contains("badge-b")
            ));


        var specJsons3Chain = new List<string>()
        {
            """
            {
              "code": "badge-a",
              "appraisal-methods": [
                {
                  "title": "Main Method",
                  "badge-requirements": [
                    "badge-b"
                  ]
                }
              ]
            }
            """,
            """
            {
              "code": "badge-b",
              "appraisal-methods": [
                {
                  "title": "Main Method",
                  "badge-requirements": [
                    "badge-c"
                  ]
                }
              ]
            }
            """,
            """
            {
              "code": "badge-c",
              "appraisal-methods": [
                {
                  "title": "Main Method",
                  "badge-requirements": [
                    "badge-a"
                  ]
                }
              ]
            }
            """,
        };

        var badgeSystem3Chain = CreateBadgeSystem(specJsons3Chain);

        Assert.IsNotNull(badgeSystem3Chain.Validations);
        Assert.AreEqual(
            1,
            badgeSystem3Chain.Errors.Count(v =>
                v.Title.Contains("Circular")
                && v.Title.Contains("badge-a")
                && v.Title.Contains("badge-b")
                && v.Title.Contains("badge-c")
            ));
    }

    [TestMethod]
    public void RepeatingDependencyValidation_MustWork()
    {
        var specJsons = new List<string>()
        {
            """
            {
              "code": "badge-a",
              "appraisal-methods": [
                {
                  "title": "Main Method",
                  "badge-requirements": [
                    "badge-b",
                    "badge-c",
                    "badge-b"
                  ]
                }
              ]
            }
            """
        };

        var badgeSystem = CreateBadgeSystem(specJsons);

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(v =>
                v.Title.Contains("Repeating dependency")
                && v.Title.Contains("badge-b")
            ));
    }

    [TestMethod]
    public void RepeatingDependencyValidationWithMultipleAppraisalMethod_MustWork()
    {
        var specJsons = new List<string>()
        {
            """
            {
              "code": "badge-a",
              "appraisal-methods": [
                {
                  "title": "Main Method",
                  "badge-requirements": [
                    "badge-b",
                    "badge-c",
                    "badge-b",
                    "badge-f"
                  ]
                },
                {
                  "title": "Second Method",
                  "badge-requirements": [
                    "badge-d",
                    "badge-c",
                    "badge-c",
                    "badge-f"
                  ]
                }
              ]
            }
            """
        };

        var badgeSystem = CreateBadgeSystem(specJsons);

        Assert.IsNotNull(badgeSystem.Validations);
        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(v =>
                v.Title.Contains("Repeating dependency")
                && v.Title.Contains("badge-c")
            ));

        Assert.AreEqual(
            1,
            badgeSystem.Errors.Count(v =>
                v.Title.Contains("Repeating dependency")
                && v.Title.Contains("badge-b")
            ));

        Assert.AreEqual(
            0,
            badgeSystem.Errors.Count(v =>
                v.Title.Contains("Repeating dependency")
                && v.Title.Contains("badge-f")
            ));
    }

    private static IBadgeSystemService CreateBadgeSystem(string specJson)
    {
        var testHost = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
                {
                    services.AddSharedServices();
                    services.AddTestServices();
                }
            ).Build();

        var badgeService = testHost.Services.GetRequiredService<IBadgeUtilService>();
        var badgeSystemService = testHost.Services.GetRequiredService<IBadgeSystemService>();

        var badge = badgeService.ParseBadge(specJson);

        Assert.IsNotNull(badge);

        var bundle = new BadgeBundleDto();
        bundle.Badges.Add(badge);

        badgeSystemService.Build(bundle);
        return badgeSystemService;
    }

    private static IBadgeSystemService CreateBadgeSystem(IEnumerable<string> specJsons)
    {
        var testHost = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
                {
                    services.AddSharedServices();
                    services.AddSingleton<ILearnerService>(new FakeLearnerService(new List<LearnerDto>()));
                }
            ).Build();

        var badgeService = testHost.Services.GetRequiredService<IBadgeUtilService>();
        var factory = testHost.Services.GetRequiredService<BadgeSystemFactory>();

        var bundle = new BadgeBundleDto();

        foreach (var specJson in specJsons)
        {
            var badge = badgeService.ParseBadge(specJson);
            Assert.IsNotNull(badge);
            bundle.Badges.Add(badge);
        }

        var badgeSystem = factory.CreateNew(bundle);
        return badgeSystem;
    }
}