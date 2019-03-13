using BookApp.Ndro.Common;
using BookApp.Ndro.View;
using BookApp.Ndro.ViewModel;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BookApp.Ndro
{
    public partial class App : Application
    {
        public NavigationPage MainContainer = null;
        public App()
        {
            InitializeComponent();
            var types = Assembly.GetExecutingAssembly().ExportedTypes;
            //types.ToList().ForEach(type =>
            //{
            //    if (typeof(Page).IsAssignableFrom(type) || typeof(BaseViewModel).IsAssignableFrom(type))
            //    {
            //        IOC.Registe(type);
            //        if (typeof(BaseViewModel).IsAssignableFrom(type) && type != typeof(BaseViewModel))
            //        {
            //            var viewType = type.GetCustomAttribute<ViewAttribute>();
            //            if (viewType != null)
            //            {
            //                ViewManager.RegisterView(viewType.ViewType, type);
            //            }
            //        }
            //    }
            //});
            IOC.Registe<MainPage>();
            IOC.Registe<MainViewModel>();
            ViewManager.RegisterView<MainPage, MainViewModel>();

            IOC.Registe<HomePage>();
            IOC.Registe<HomeViewModel>();
            ViewManager.RegisterView<HomePage, HomeViewModel>();

            IOC.Registe<LocalShelfPage>();
            IOC.Registe<LocalShelfViewModel>();
            ViewManager.RegisterView<LocalShelfPage, LocalShelfViewModel>();

            IOC.Registe<SearchPage>();
            IOC.Registe<SearchViewModel>();
            ViewManager.RegisterView<SearchPage, SearchViewModel>();

            IOC.Registe<BookSourcePage>();
            IOC.Registe<BookSourceViewModel>();
            ViewManager.RegisterView<BookSourcePage, BookSourceViewModel>();

            IOC.Registe<SettingPage>();
            IOC.Registe<SettingViewModel>();
            ViewManager.RegisterView<SettingPage, SettingViewModel>();


            IOC.Build();
            var homePage = ViewManager.CreateView<HomePage>();
            var mainPage = ViewManager.CreateView<MainPage>();
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
