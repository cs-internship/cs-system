using AutoMapper;
using CrystallineSociety.Server.Api.Models;
using CrystallineSociety.Server.Models;

namespace CrystallineSociety.Server.Mappers;

public class ProgramDocumentMapperConfiguration : Profile
{
    public ProgramDocumentMapperConfiguration()
    {
        CreateMap<ProgramDocument, ProgramDocumentDto>().ReverseMap();
    }
}
