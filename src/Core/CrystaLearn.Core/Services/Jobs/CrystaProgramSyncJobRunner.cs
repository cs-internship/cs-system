using System.Threading;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services.Jobs;

public partial class CrystaProgramSyncJobRunner
{
    [AutoInject] private ICrystaProgramSyncModuleService syncModuleService = default!;
    [AutoInject] private ICrystaProgramSyncService syncService = default!;

    private static bool isRunning = false;

    public async Task RunSyncForAllModules(CancellationToken cancellationToken)
    {

        try
        {
            if (isRunning)
            {
                return;
            }

            Console.Write($"Sync Start at {DateTime.UtcNow}");
            
            isRunning = true;
            var modules = await syncModuleService.GetSyncModulesAsync(cancellationToken);

            int i = 0;
            while (i < modules.Count)
            {
                await syncService.SyncAsync(modules[i]);
                i++;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during sync: {ex.Message} {ex}");
            isRunning = false;
            throw;
        }

        isRunning = false;

    }
}
