using Dima.Core.Handlers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;

namespace Dima.Web.Components.Reports
{
    public partial class IncomesAndExpensesChartComponents : ComponentBase
    {
        #region Properties
        public ChartOptions Options { get; set; } = new();
        public List<ChartSeries>? Series { get; set; }
        public List<string> Labels { get; set; } = [];
        #endregion

        #region Services
        [Inject]
        public IReportHandler Handler { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            var result = await Handler.GetIncomesAndExpensesReportAsync(new Core.Requests.Reports.GetIncomesAndExpensesRequest());
            if(!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add("Falha ao obter os dados do relatório", Severity.Error);
                return;
            }

            var incomes = new List<double>();
            var expenses = new List<double>();

            foreach (var item in result.Data)
            {
                incomes.Add((double)item.Incomes);
                expenses.Add(-(double)item.Expenses);
                Labels.Add(GetMonthName(item.Month));
            }

            Options.YAxisTicks = 1000;
            Options.LineStrokeWidth = 5;
            Options.ChartPalette = ["#76FF01", Colors.Red.Default];
            Series = [
                new ChartSeries{Name = "Receitas", Data=incomes.ToArray()},
                new ChartSeries{Name = "Despesas", Data=expenses.ToArray()}
                ];
            StateHasChanged();
        }
        #endregion

        private static string GetMonthName(int month)
            => new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM", CultureInfo.CurrentCulture);
    }
}
