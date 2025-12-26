using System;
using System.Collections.Generic;
using System.Linq;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class CrystaProgramSyncModuleServiceFake : ICrystaProgramSyncModuleService
{
    private static List<CrystaProgramSyncModule> _modules = new();

    private IConfiguration Configuration { get; set; } = default!;

    public CrystaProgramSyncModuleServiceFake(IConfiguration configuration)
    {
        Configuration = configuration;
        if (_modules.Count == 0)
        {
            var pat = Configuration["AzureDevOps:PersonalAccessToken"];

            _modules = new List<CrystaProgramSyncModule>
            {
                new CrystaProgramSyncModule
                {
                    Id = Guid.NewGuid(),
                    CrystaProgramId = CrystaProgramServiceFake.FakeProgramCSI.Id,
                    CrystaProgram = CrystaProgramServiceFake.FakeProgramCSI,
                    ModuleType = SyncModuleType.AzureBoard,
                   SyncConfig =
                          $$"""
                            {
                                "Organization": "cs-internship",
                                "PersonalAccessToken": "{{pat}}",
                                "Project": "CS Internship Program"
                            }
                            """,
                    SyncInfo = new SyncInfo
                    {
                        LastSyncDateTime = DateTimeOffset.Now.AddDays(-2),
                        LastSyncOffset = "0"
                    }
                }
            };
        }
    }

    public async Task<List<CrystaProgramSyncModule>> GetSyncModulesAsync(CancellationToken cancellationToken)
    {
        return _modules;
    }

    public async Task UpdateSyncModuleAsync(CrystaProgramSyncModule module)
    {
        var existing = _modules.FirstOrDefault(m => m.Id == module.Id);
        if (existing != null)
        {
            existing.SyncInfo = module.SyncInfo;
            existing.SyncConfig = module.SyncConfig;
        }
        else
        {
            _modules.Add(module);
        }
    }
}
