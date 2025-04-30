using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystaLearn.Core.Services.Sync;

namespace CrystaLearn.Core.Services.Contracts;

public interface ICrystaTaskRepository
{
    Task<List<SyncItem>> GetWorkItemSyncItemsAsync(List<string> ids);
}
