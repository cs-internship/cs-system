using CrystaLearn.Server.Api.Models.Crysta;
using CrystaLearn.Server.Api.Models.Identity;
using CrystaLearn.Server.Api.Models.PushNotification;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Identity;
using CrystaLearn.Shared.Dtos.PushNotification;
using Riok.Mapperly.Abstractions;

namespace CrystaLearn.Server.Api.Mappers;

[Mapper]
public static partial class CrystaProgramMapper
{
    public static partial CrystaProgramDto Map(this CrystaProgram source);
    public static partial CrystaProgramLightDto MapLight(this CrystaProgram source);
}
