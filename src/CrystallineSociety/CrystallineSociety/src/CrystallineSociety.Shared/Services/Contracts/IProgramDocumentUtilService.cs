using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Shared.Services.Contracts;
public interface IProgramDocumentUtilService
{
    ProgramDocumentDto ParseProgramDocument(string specJson);
    string SerializeProgramDocument(ProgramDocumentDto programDocument);
}
