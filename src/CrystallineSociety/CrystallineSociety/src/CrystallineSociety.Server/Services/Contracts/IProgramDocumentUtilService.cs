namespace CrystallineSociety.Server.Services.Contracts;
public interface IProgramDocumentUtilService
{
    ProgramDocumentDto ParseProgramDocument(string specJson);
    string SerializeProgramDocument(ProgramDocumentDto programDocument);
}
