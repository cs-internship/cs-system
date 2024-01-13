using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CrystallineSociety.Shared.Json.Converters;

using CrystallineSociety.Shared.Utils;
using IProgramDocumentUtilService = CrystallineSociety.Server.Services.Contracts.IProgramDocumentUtilService;

namespace CrystallineSociety.Shared.Services.Implementations.ProgramDocument;
public class ProgramDocumentUtilService : IProgramDocumentUtilService
{
    private static JsonSerializerOptions ProgramDocumentSerializerOptions { get; set; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = KebabCaseNamingPolicy.Instance,
        WriteIndented = true,
        Converters =
            {
                new JsonStringEnumConverter(),
            }
    };

    public ProgramDocumentDto ParseProgramDocument(string specJson)
    {
        var programDocument = JsonSerializer.Deserialize<ProgramDocumentDto>(specJson, ProgramDocumentSerializerOptions);

        if (programDocument is null)
            throw new InvalidOperationException("Can not create program document from spec.");

        programDocument.Title ??= programDocument.Code;

        return programDocument;
    }

    public string SerializeProgramDocument(ProgramDocumentDto programDocument)
    {
        return JsonSerializer.Serialize(programDocument, ProgramDocumentSerializerOptions);
    }
}
