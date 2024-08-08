using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories
{
    public partial class ListCategoriesPage : ComponentBase
    {
        #region Properties
        public bool IsBusy { get; set; }
        public List<Category> Categories { get; set; } = [];
        public string SearchTerm { get; set; } = string.Empty;
        #endregion

        #region services
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        [Inject]
        public ICategoryHandler Handler { get; set; } = null!;
        #endregion

        #region overrides
        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;
            try
            {
                var request = new GetAllCategoriesRequest();
                var result = await Handler.GetAllAsync(request);
                if (result.IsSuccess)
                    Categories = result.Data ?? [];
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally { IsBusy = false; }
        }
        #endregion

        #region Methods

        public async void OnDeleteButtonClickedAsync(long id, string title)
        {
            var result = await DialogService.ShowMessageBox("Atenção", $"Ao prosseguir a categoria {title} será excluída. " +
                $"Esta é uma ação irreversível, deseja prosseguir?", "Excluir", cancelText: "Cancelar");

            if (result is true)
                await OnDeleteAsync(id, title);

            StateHasChanged();
        }

        public async Task OnDeleteAsync(long id, string title)
        {
            IsBusy = true;
            try
            {
                await Handler.DeleteAsync(new DeleteCategoryRequest { Id = id });
                Categories.RemoveAll(x => x.Id == id);
                Snackbar.Add($"Categoria {title} excluída!", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally { IsBusy = false; }
        }

        public Func<Category, bool> Filter => category =>
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            if (category.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if(category.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;

            if (category.Description is not null && category.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                return true;


            return false;
        };
        #endregion
    }
}
