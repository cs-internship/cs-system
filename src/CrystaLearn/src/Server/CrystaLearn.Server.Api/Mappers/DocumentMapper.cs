using CrystaLearn.Server.Api.Models.Crysta;
using CrystaLearn.Server.Api.Models.Identity;
using CrystaLearn.Server.Api.Models.PushNotification;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Identity;
using CrystaLearn.Shared.Dtos.PushNotification;
using Riok.Mapperly.Abstractions;

namespace CrystaLearn.Server.Api.Mappers;

[Mapper]
public static partial class DocumentMapper
{
    public static partial DocumentDto Map(this Document source);
}
