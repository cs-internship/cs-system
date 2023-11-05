using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystallineSociety.Client.Shared.Pages
{
    public partial class OrganizationPage
    {
        [Parameter]
        public string OrganizationCode { get; set; }

        private OrganizationDto Organization { get; set; }

        protected override async Task OnInitAsync()
        {
            await LoadOrganizationAsync();
            await base.OnInitAsync();
        }

        private async Task LoadOrganizationAsync()
        {
            Organization = new OrganizationDto(){Code = "CSI", Title = "CS Internship", Description = "Our first program"};
        }
    }
}
