using BookApp.Ndro.View;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using BookApp.Ndro.Common;

namespace BookApp.Ndro.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Page> _tabPages = new ObservableCollection<Page>();
        private Page _currentTabPage;

        public HomeViewModel()
        {
            TabPages.Add(ViewManager.CreateView<LocalShelfPage>());
            CurrentTabPage = TabPages.First();
        }

        public ObservableCollection<Page> TabPages { get => _tabPages; set => Set(ref _tabPages, value); }

        public Page CurrentTabPage { get => _currentTabPage; set => Set(ref _currentTabPage, value); }
    }
}
