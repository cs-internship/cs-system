using AutoMapper;
using CrystallineSociety.Server.Api.Models;

namespace CrystallineSociety.Server.Mappers;

public class ProgramDocumentMapperConfiguration : Profile
{
    public ProgramDocumentMapperConfiguration()
    {
        CreateMap<ProgramDocument, ProgramDocumentDto>().ReverseMap();
    }
}
