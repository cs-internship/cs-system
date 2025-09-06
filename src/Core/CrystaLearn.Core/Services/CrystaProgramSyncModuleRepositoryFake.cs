using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class CrystaProgramSyncModuleRepositoryFake : ICrystaProgramSyncModuleRepository
{
    [AutoInject] private IConfiguration Configuration { get; set; } = default!;

    public async Task<List<CrystaProgramSyncModule>> GetSyncModulesAsync(CancellationToken cancellationToken)
    {
        var pat = Configuration["AzureDevOps:PersonalAccessToken"];

        List<CrystaProgramSyncModule> modules =
        [
            new()
            {
                Id = Guid.NewGuid(),
                CrystaProgramId = CrystaProgramRepositoryFake.FakeProgramCSI.Id,
                CrystaProgram = CrystaProgramRepositoryFake.FakeProgramCSI,
                ModuleType = SyncModuleType.AzureBoard,
                SyncConfig =
                    $$"""
                    {   
                        "Organization": "cs-internship",
                        "PersonalAccessToken": "{{pat}}",
                        "Project": "CS Internship Program"
                    }
                    """,
                SyncInfo = new()
                {
                    LastSyncDateTime = DateTimeOffset.Now.AddDays(-2),
                    LastSyncOffset = "0"
                }
            }
        ];

        return modules;
    }
}
