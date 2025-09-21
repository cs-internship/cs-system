using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;
using Riok.Mapperly.Abstractions;

namespace CrystaLearn.Core.Mappers;

[Mapper]
public static partial class CrystaProgramMapper
{
    public static partial CrystaProgramDto Map(this CrystaProgram source);
    public static partial CrystaProgramLightDto MapLight(this CrystaProgram source);
}
