using System;
using System.Collections.Generic;
using System.Linq;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class CrystaProgramSyncModuleServiceFake : ICrystaProgramSyncModuleService
{
    private List<CrystaProgramSyncModule> _modules = new();
    private bool _initialized = false;
    private readonly SemaphoreSlim _initLock = new SemaphoreSlim(1, 1);
    private readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1, 1);

    private IConfiguration Configuration { get; set; } = default!;

    public CrystaProgramSyncModuleServiceFake(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private async Task EnsureInitializedAsync()
    {
        if (!_initialized)
        {
            await _initLock.WaitAsync();
            try
            {
                if (!_initialized)
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
                    _initialized = true;
                }
            }
            finally
            {
                _initLock.Release();
            }
        }
    }

    public async Task<List<CrystaProgramSyncModule>> GetSyncModulesAsync(CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        return _modules;
    }

    public async Task UpdateSyncModuleAsync(CrystaProgramSyncModule module, CancellationToken cancellationToken = default)
    {
        await _updateLock.WaitAsync(cancellationToken);
        try
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
        finally
        {
            _updateLock.Release();
        }
    }
}
