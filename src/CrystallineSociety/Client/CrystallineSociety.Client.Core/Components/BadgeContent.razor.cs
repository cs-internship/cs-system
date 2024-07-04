using System;
using Microsoft.AspNetCore.Components;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeContent : ComponentBase
    {
        [Parameter] public BadgeDto? Badge { get; set; }
        private AppraisalMethod? selectedAppraisalMethod;

        protected override void OnParametersSet()
        {
            if (Badge?.AppraisalMethods?.Any() == true)
            {
                selectedAppraisalMethod = Badge.AppraisalMethods.First();
            }
            else
            {
                selectedAppraisalMethod = null;
            }
        }

        private void SelectAppraisalMethod(AppraisalMethod appraisalMethod)
        {
            selectedAppraisalMethod = appraisalMethod;
        }
    }
}
