using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Api.Models.TodoItem;
using CrystallineSociety.Shared.Dtos.Organization;
using CrystallineSociety.Shared.Dtos.TodoItem;

namespace CrystallineSociety.Server.Api.Mappers;

public class OrganizationMapperConfiguration : Profile
{
    public OrganizationMapperConfiguration()
    {
        CreateMap<Organization, OrganizationDto>().ReverseMap();
    }
}
