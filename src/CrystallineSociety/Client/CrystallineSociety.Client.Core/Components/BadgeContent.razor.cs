using System;
using Microsoft.AspNetCore.Components;
using CrystallineSociety.Shared.Dtos.BadgeSystem;

namespace CrystallineSociety.Client.Core.Components
{
    public partial class BadgeContent : ComponentBase
    {
        [Parameter] public BadgeDto? Badge { get; set; }
        private AppraisalMethod selectedAppraisalMethod;

        protected override void OnInitialized()
        {
            if (Badge?.AppraisalMethods?.Any() == true)
            {
                selectedAppraisalMethod = Badge.AppraisalMethods.First();
            }
        }

        private void SelectAppraisalMethod(AppraisalMethod appraisalMethod)
        {
            selectedAppraisalMethod = appraisalMethod;
        }
    }
}
