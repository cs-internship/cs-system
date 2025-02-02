using CrystaLearn.Core.Models.Crysta;
using CrystaLearn.Shared.Dtos.Crysta;
using Riok.Mapperly.Abstractions;

namespace CrystaLearn.Core.Mappers;

[Mapper]
public static partial class SyncInfoMapper
{
    public static partial SyncInfoDto Map(this SyncInfo source);
}
