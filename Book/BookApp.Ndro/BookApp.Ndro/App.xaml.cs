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
                    if (typeof(BaseViewModel).IsAssignableFrom(type))
                    {
                        var viewType = type.GetCustomAttribute<ViewAttribute>();
                        if (viewType != null)
                        {
                            ViewManager.RegisterView(viewType.ViewType, type);
                        }
                    }
                }
            });
            IOC.Build();
            var homePage = ViewManager.CreateView<HomePage>();
            var mainPage = new NavigationPage(homePage);
            mainPage.HeightRequest = 20;
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
