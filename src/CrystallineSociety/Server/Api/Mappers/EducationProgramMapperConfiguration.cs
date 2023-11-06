using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Api.Models.TodoItem;
using CrystallineSociety.Shared.Dtos.Organization;
using CrystallineSociety.Shared.Dtos.TodoItem;

namespace CrystallineSociety.Server.Api.Mappers;

public class EducationProgramMapperConfiguration : Profile
{
    public EducationProgramMapperConfiguration()
    {
        CreateMap<Organization, OrganizationDto>().ReverseMap();
    }
}
