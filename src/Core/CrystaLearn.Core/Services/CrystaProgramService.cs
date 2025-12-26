using CrystaLearn.Core.Data;
using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class CrystaProgramService : ICrystaProgramRepository
{
    [AutoInject] private AppDbContext DbContext { get; set; } = default!;

    public async Task<List<CrystaProgram>> GetCrystaProgramsAsync(CancellationToken cancellationToken)
    {
        return await DbContext.CrystaPrograms
            .Where(p => p.IsActive)
            .OrderBy(p => p.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<CrystaProgram?> GetCrystaProgramByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await DbContext.CrystaPrograms
            .Where(p => p.Code == code && p.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
