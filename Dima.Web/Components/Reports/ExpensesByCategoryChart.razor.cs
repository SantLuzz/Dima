using Dima.Core.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports
{
    public partial class ExpensesByCategoryChartComponents : ComponentBase
    {
        #region Property
        public List<double> Data { get; set; } = [];
        public List<string> Labels { get; set; } = [];
        #endregion

        #region services
        [Inject]
        public IReportHandler Handler { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region overrides
        protected override async Task OnInitializedAsync()
        {
            await GetExpensesByCategoryAsync();
        }

        private async Task GetExpensesByCategoryAsync()
        {
            var result = await Handler.GetExpensesByCategoryReportAsync(new Core.Requests.Reports.GetExpensesByCategoryRequest());

            if(!result.IsSuccess && result.Data is null)
            {
                Snackbar.Add("Falha ao obter os dados do relatório", Severity.Error);
                return;
            }

            foreach (var item in result?.Data ?? [])
            {
                Labels.Add($"{item.Category} ({item.Expenses:C})");
                Data.Add(-(double)item.Expenses);
            }


        }
        #endregion
    }
}
