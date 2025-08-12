using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.PushNotification;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Identity;
using CrystaLearn.Shared.Dtos.PushNotification;
using Riok.Mapperly.Abstractions;

namespace CrystaLearn.Core.Mappers;

[Mapper]
public static partial class CrystaProgramMapper
{
    public static partial CrystaProgramDto Map(this CrystaProgram source);
    public static partial CrystaProgramLightDto MapLight(this CrystaProgram source);
}
