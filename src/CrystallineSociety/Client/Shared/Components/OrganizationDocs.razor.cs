using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrystallineSociety.Shared.Dtos.BadgeSystem;
using CrystallineSociety.Shared.Dtos.Organization;

namespace CrystallineSociety.Client.Shared.Components
{
    public partial class OrganizationDocs
    {
        [Parameter]
        public OrganizationDto Organization { get; set; } = default!;

        private List<ProgramDocumentDto> _programDocuments { get; set; } = new();

        private ProgramDocumentDto? _programDocument { get; set; }

        protected override async Task OnInitAsync()
        {
            _programDocuments = await HttpClient.GetFromJsonAsync($"ProgramDocument/GetProgramDocuments?organizationCode={Organization.Code}", AppJsonContext.Default.ListProgramDocumentDto) ?? _programDocuments;
            await base.OnInitAsync();
        }

        private void GetProgramDocument(ProgramDocumentDto programDocument)
        {
            _programDocument = programDocument;
        }
    }
}
