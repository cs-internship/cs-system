using AutoMapper;
using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Models;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Server.Mappers;

public class BadgeMapperConfiguration : Profile
{
    public BadgeMapperConfiguration()
    {
        CreateMap<Badge, BadgeDto>().ReverseMap();
    }
}
