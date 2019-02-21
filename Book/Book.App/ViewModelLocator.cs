using Book.App.View;
using Book.App.ViewModel;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace Book.App
{
    internal class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var navigationService = this.InitNavigationService();
            SimpleIoc.Default.Register(() => navigationService);
            SimpleIoc.Default.Register<MainViewModel>();
        }

        private INavigationService InitNavigationService()
        {
            NavigationService navigationService = new NavigationService();
            navigationService.Configure("Main", typeof(MainView));
            return navigationService;
        }

        private static MainViewModel _main;
        public static MainViewModel Main
        {
            get
            {
                if (_main == null)
                {
                    _main = ServiceLocator.Current.GetInstance<MainViewModel>();
                }

                return _main;
            }
        }

        private static SearchViewModel _search;
        public static SearchViewModel Search
        {
            get
            {
                if (_search == null)
                {
                    _search = ServiceLocator.Current.GetInstance<SearchViewModel>();
                }

                return _search;
            }
        }
    }
}
