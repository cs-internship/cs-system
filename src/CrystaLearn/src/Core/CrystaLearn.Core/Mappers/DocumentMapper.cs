using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Core.Models.Identity;
using CrystaLearn.Core.Models.PushNotification;
using CrystaLearn.Shared.Dtos.Crysta;
using CrystaLearn.Shared.Dtos.Identity;
using CrystaLearn.Shared.Dtos.PushNotification;
using Riok.Mapperly.Abstractions;

namespace CrystaLearn.Core.Mappers;

[Mapper]
public static partial class DocumentMapper
{
    public static partial DocumentDto Map(this Document source);
}
