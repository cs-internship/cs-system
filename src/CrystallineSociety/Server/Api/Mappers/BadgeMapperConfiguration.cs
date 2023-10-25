using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Api.Mappers;

public class BadgeMapperConfiguration : Profile
{
    public BadgeMapperConfiguration()
    {
        CreateMap<Badge, BadgeDto>().ReverseMap();
    }
}
