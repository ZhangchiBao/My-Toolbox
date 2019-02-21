using Book.App.Model;
using Book.App.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Book.App.ViewModel
{
    public class MainViewModel : VMBase
    {
        private string _keyword;
        private ObservableCollection<NavigationItemModel> _navigationItemList;
        private NavigationItemModel _selectedNavigationItem;
        private ICommand _doSearchCommand;

        public MainViewModel()
        {
            NavigationItemList = new ObservableCollection<NavigationItemModel>();
            NavigationItemList.Add(new NavigationItemModel
            {
                Icon = Symbol.Link,
                Tag = "Search",
                Title = "在线搜索",
                ViewType = typeof(SearchView)
            });
            NavigationItemList.Add(new NavigationItemModel
            {
                Icon = Symbol.Library,
                Tag = "Shelf",
                Title = "本地书架"
            });
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedNavigationItem):
                    View.Navigate(SelectedNavigationItem.ViewType, SelectedNavigationItem.Title);
                    break;
                case nameof(Keyword):
                    RaisePropertyChanged(nameof(DoSearchCommand));
                    break;
            }
        }

        public string Keyword { get => _keyword; set => Set(ref _keyword, value); }

        public ObservableCollection<NavigationItemModel> NavigationItemList { get => _navigationItemList; set => Set(ref _navigationItemList, value); }

        public NavigationItemModel SelectedNavigationItem { get => _selectedNavigationItem; set => Set(ref _selectedNavigationItem, value); }

        public MainView View { get; set; }

        public ICommand DoSearchCommand { get => _doSearchCommand; set => Set(ref _doSearchCommand, value); }
    }
}
