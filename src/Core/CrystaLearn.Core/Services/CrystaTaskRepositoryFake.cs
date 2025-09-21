using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services;

public class CrystaTaskRepositoryFake : ICrystaTaskRepository
{
    public async Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids)
    {
        return [];
    }
}
