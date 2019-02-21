using Book.App.ViewModel;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Book.App.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
            Loaded += (s, e) =>
            {
                MainViewModel dataContext = (MainViewModel)DataContext;
                dataContext.View = this;
                dataContext.SelectedNavigationItem = dataContext.NavigationItemList.First();
            };
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        public void Navigate(Type viewType, string header)
        {
            NVView.Header = header;
            if (viewType != null)
            {
                NVContent.Navigate(viewType);
            }
        }
    }
}
