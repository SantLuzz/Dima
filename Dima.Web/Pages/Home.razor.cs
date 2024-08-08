using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages
{
    public partial class HomePage : ComponentBase
    {
        #region Properties
        public bool ShowValues { get; set; } = true;
        public FinancialSummary? Summary { get; set; }
        #endregion

        #region Services
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        public IReportHandler Handler { get; set; } = null!;
        #endregion

        #region overrides
        protected override async Task OnInitializedAsync()
        {
            var result = await Handler.GetFinancialSummaryReportAsync(new Core.Requests.Reports.GetFinancialSummaryRequest());
            if (result.IsSuccess)
                Summary = result.Data;
        }
        #endregion

        #region Methods
        public void ToggleShowValues() => ShowValues = !ShowValues;
        #endregion
    }
}
