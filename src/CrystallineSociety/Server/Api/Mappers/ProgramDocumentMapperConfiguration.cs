using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Server.Api.Mappers;

public class ProgramDocumentMapperConfiguration : Profile
{
    public ProgramDocumentMapperConfiguration()
    {
        CreateMap<ProgramDocument, ProgramDocumentDto>().ReverseMap();
    }
}
