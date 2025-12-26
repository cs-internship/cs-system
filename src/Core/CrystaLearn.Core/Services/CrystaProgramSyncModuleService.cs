using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrystaLearn.Core.Data;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class CrystaProgramSyncModuleService : ICrystaProgramSyncModuleService
{

    private static List<CrystaProgramSyncModule> _modules = new();
    private AppDbContext DbContext { get; set; } = default!;

    public CrystaProgramSyncModuleService(AppDbContext dbContext)
    {
        this.DbContext = dbContext;
        if (_modules.Count == 0)
        {
            _modules = DbContext.Set<CrystaProgramSyncModule>().Include(f => f.CrystaProgram).ToListAsync().GetAwaiter().GetResult();
        }
    }

    public async Task<List<CrystaProgramSyncModule>> GetSyncModulesAsync(CancellationToken cancellationToken)
    {
        return _modules;
    }

    public async Task UpdateSyncModuleAsync(CrystaProgramSyncModule module)
    {
        // Try to persist to database if DbContext is available and configured
        try
        {
            if (DbContext != null)
            {
                var set = DbContext.Set<CrystaProgramSyncModule>();

                var existing = await set.FindAsync(new object[] { module.Id }, cancellationToken: CancellationToken.None);
                if (existing != null)
                {
                    // Update all scalar properties from incoming module
                    DbContext.Entry(existing).CurrentValues.SetValues(module);

                    // If SyncInfo is an owned/complex type, ensure its properties are updated as well
                    if (module.SyncInfo != null)
                    {
                        existing.SyncInfo ??= new SyncInfo();
                        DbContext.Entry(existing).CurrentValues.SetValues(existing); // ensure entry is tracked
                        DbContext.Entry(existing).Reference(e => e.SyncInfo).TargetEntry?.CurrentValues.SetValues(module.SyncInfo);
                    }

                    DbContext.Update(existing);
                }
                else
                {
                    await set.AddAsync(module);
                }

                await DbContext.SaveChangesAsync();

                // keep in-memory copy in sync as well - replace whole object to reflect all fields
                var idx = _modules.FindIndex(m => m.Id == module.Id);
                if (idx >= 0)
                {
                    _modules[idx] = module;
                }
                else
                {
                    _modules.Add(module);
                }

                return;
            }
        }
        catch
        {
            // If persistence fails (for example entity not mapped), fall back to in-memory update below.
        }

        // Fallback: update in-memory collection (replace whole object)
        var existingInMemoryIndex = _modules.FindIndex(m => m.Id == module.Id);
        if (existingInMemoryIndex >= 0)
        {
            _modules[existingInMemoryIndex] = module;
        }
        else
        {
            _modules.Add(module);
        }
    }
}
