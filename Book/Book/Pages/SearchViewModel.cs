using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Stylet;
using StyletIoC;

namespace Book.Pages
{
    public class SearchViewModel : Screen
    {
        private readonly IViewManager viewManager;
        private readonly IContainer container;

        public SearchViewModel(IViewManager viewManager, IContainer container)
        {
            this.viewManager = viewManager;
            this.container = container;
        }

        public string SearchKeyword { get; set; }

        public bool CanDoSearch => !string.IsNullOrEmpty(SearchKeyword);

        public async void CloseWindow()
        {
            var dialog = (CustomDialog)View;
            await ((MetroWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>())).HideMetroDialogAsync(dialog);
        }

        public void DoSearch()
        {

        }
    }
}
