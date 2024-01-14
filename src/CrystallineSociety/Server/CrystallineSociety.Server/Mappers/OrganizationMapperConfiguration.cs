using AutoMapper;
using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Server.Mappers;

public class OrganizationMapperConfiguration : Profile
{
    public OrganizationMapperConfiguration()
    {
        CreateMap<Organization, OrganizationDto>().ReverseMap();
    }
}
