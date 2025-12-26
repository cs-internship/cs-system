using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrystaLearn.Core.Data;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CrystaLearn.Core.Services;

public partial class CrystaProgramSyncModuleService : ICrystaProgramSyncModuleService
{

    private List<CrystaProgramSyncModule> _modules = new();
    private IDbContextFactory<AppDbContext> DbContextFactory { get; set; } = default!;
    private bool _initialized = false;
    private readonly SemaphoreSlim _initLock = new SemaphoreSlim(1, 1);
    private readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1, 1);

    public CrystaProgramSyncModuleService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        this.DbContextFactory = dbContextFactory;
    }

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
    {
        if (!_initialized)
        {
            await _initLock.WaitAsync(cancellationToken);
            try
            {
                if (!_initialized)
                {
                    await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
                    _modules = await dbContext.Set<CrystaProgramSyncModule>().Include(f => f.CrystaProgram).ToListAsync(cancellationToken);
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
        await EnsureInitializedAsync(cancellationToken);
        return _modules;
    }

    public async Task UpdateSyncModuleAsync(CrystaProgramSyncModule module)
    {
        // Try to persist to database if DbContext is available and configured
        try
        {
            if (DbContextFactory != null)
            {
                await using var dbContext = await DbContextFactory.CreateDbContextAsync(CancellationToken.None);
                var set = dbContext.Set<CrystaProgramSyncModule>();

                var existing = await set.FindAsync(new object[] { module.Id }, cancellationToken: CancellationToken.None);
                if (existing != null)
                {
                    // Update all scalar properties from incoming module
                    dbContext.Entry(existing).CurrentValues.SetValues(module);

                    // If SyncInfo is an owned/complex type, ensure its properties are updated as well
                    if (module.SyncInfo != null)
                    {
                        existing.SyncInfo ??= new SyncInfo();
                        dbContext.Entry(existing).CurrentValues.SetValues(existing); // ensure entry is tracked
                        dbContext.Entry(existing).Reference(e => e.SyncInfo).TargetEntry?.CurrentValues.SetValues(module.SyncInfo);
                    }

                    dbContext.Update(existing);
                }
                else
                {
                    await set.AddAsync(module);
                }

                await dbContext.SaveChangesAsync();

                // keep in-memory copy in sync as well - replace whole object to reflect all fields
                await _updateLock.WaitAsync();
                try
                {
                    var idx = _modules.FindIndex(m => m.Id == module.Id);
                    if (idx >= 0)
                    {
                        _modules[idx] = module;
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

                return;
            }
        }
        catch
        {
            // If persistence fails (for example entity not mapped), fall back to in-memory update below.
        }

        // Fallback: update in-memory collection (replace whole object)
        await _updateLock.WaitAsync();
        try
        {
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
        finally
        {
            _updateLock.Release();
        }
    }
}
