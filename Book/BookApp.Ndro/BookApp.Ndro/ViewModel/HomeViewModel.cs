using BookApp.Ndro.Common;
using BookApp.Ndro.View;
using BookAPP.Entity;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace BookApp.Ndro.ViewModel
{
    [View(typeof(HomePage))]
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Page> _tabPages = new ObservableCollection<Page>();
        private Page _currentTabPage;

        public HomeViewModel()
        {
            TabPages.Add(ViewManager.CreateView<LocalShelfPage>());
            TabPages.Add(ViewManager.CreateView<SettingPage>());
            CurrentTabPage = TabPages.First();
        }

        public ObservableCollection<Page> TabPages { get => _tabPages; set => Set(ref _tabPages, value); }

        public Page CurrentTabPage { get => _currentTabPage; set => Set(ref _currentTabPage, value); }

        public Command SearchCommand => new Command(GotoSearchViewAsync);

        private async void GotoSearchViewAsync()
        {
            var searchViewModel = IOC.Get<SearchViewModel>();
            searchViewModel.Keyword = "诛仙";
            searchViewModel.Data = new ObservableCollection<BookModel>();
            searchViewModel.LoadStatus = Control.LoadMoreStatus.StatusDefault;
            await View.Navigation.PushAsync(ViewManager.CreateView<SearchPage>());
        }
    }
}
