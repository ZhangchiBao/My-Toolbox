using BookApp.Ndro.View;
using BookApp.Ndro.ViewModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BookApp.Ndro.Common;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BookApp.Ndro
{
    public partial class App : Application
    {
        public NavigationPage MainContainer = null;
        public App()
        {
            InitializeComponent();
            Assembly assembly = Assembly.GetExecutingAssembly();
            assembly.ExportedTypes.ToList().ForEach(type =>
            {
                if (typeof(Page).IsAssignableFrom(type) || typeof(BaseViewModel).IsAssignableFrom(type))
                {
                    IOC.Registe(type);
                }
            });
            ViewManager.RegisterView<HomePage, HomeViewModel>();
            ViewManager.RegisterView<LocalShelfPage, LocalShelfViewModel>();
            ViewManager.RegisterView<SearchPage, SearchViewModel>();
            IOC.Build();
            var page = ViewManager.CreateView<HomePage>();
            MainPage = new NavigationPage(page);
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
