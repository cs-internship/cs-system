using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystaLearn.Core.Services.Contracts;
public interface IAzureBoardService
{
    Task GetWorkItemsAsync();
}
