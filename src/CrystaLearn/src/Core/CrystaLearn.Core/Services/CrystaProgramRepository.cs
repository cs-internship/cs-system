using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Services.Contracts;

namespace CrystaLearn.Core.Services;

public partial class CrystaProgramRepositoryFake : ICrystaProgramRepository
{
    public static List<CrystaProgram> FakePrograms { get; set; } =
    [
        new()
        {
            Id = new Guid("C73198A2-5913-40D0-AC28-F5306CCD7534"),
            Code = "cs-internship",
            Title = "CS Internship",
            IsActive = true,
        },

        new()
        {
            Id = new Guid("03236DE7-01DD-4B11-8328-612A1AACACA5"),
            Code = "melkradar",
            Title = "MelkRadar",
            IsActive = true,
        }

    ];

    public async Task<List<CrystaProgram>> GetCrystaProgramsAsync(CancellationToken cancellationToken)
    {
        return FakePrograms;
    }

    public async Task<CrystaProgram?> GetCrystaProgramByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return FakePrograms.FirstOrDefault(p => p.Code == code);
    }
}
