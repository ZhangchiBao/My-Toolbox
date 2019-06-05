using BookReading.BrowserHandlers;
using BookReading.Entities;
using BookReading.Libs;
using BookReading.Views;
using CefSharp;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookReading.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        private MenuHandler menuHandler;

        public ShellViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager, BookContext db) : base(container, windowManager, viewManager, db)
        {
            Task.Run(LoadBookShelf);
        }

        public ObservableCollection<CategoryShowModel> ShlefData { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        private BookShowModel currentBook;

        public string ShowFile { get; set; }

        /// <summary>
        /// 弹出搜索窗口
        /// </summary>
        public void ShowSearchView()
        {
            var vm = container.Get<SearchViewModel>();
            vm.Keyword = Keyword;
            if (!string.IsNullOrWhiteSpace(vm.Keyword))
            {
                //vm.DoSearch();
                vm.ViewLoaded += vm.DoSearch;
            }
            if (windowManager.ShowDialog(vm) ?? false)
            {
                Task.Run(LoadBookShelf);
            }
        }

        public void Keyword_Inputbox_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    ShowSearchView();
                }
            }
        }

        public void Shelf_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            menuHandler.MenuItems.Clear();
            if (e.OriginalSource is System.Windows.FrameworkElement element && element.DataContext is BookShowModel book)
            {
                currentBook = book;
                ShowFile = Path.Combine(App.BOOKSHELF_FLODER, book.BookFloder, "List.htm");
                menuHandler.MenuItems.Add("测试菜单1", null);
                menuHandler.MenuItems.Add("测试菜单2", null);
                if (View is ShellView view)
                {
                    //view.WebBrowser.RegisterJsObject("wpfObj", new CategotyCallbackObjectForJs(book, this), new BindingOptions { CamelCaseJavascriptNames = false });
                }
            }
            else
            {
            }
        }

        public void GotoChapter(int index)
        {
            var chapter = currentBook.Chapters[index];
            if (string.IsNullOrEmpty(chapter.FilePath))
            {
                var finder = container.GetAll<IFinder>().Single(a => a.FinderKey == chapter.FinderKey);
            }
        }

        /// <summary>
        /// 加载书架
        /// </summary>
        private void LoadBookShelf()
        {
            var data = db.Categories.Include(a => a.Books).Include(a => a.Books.Select(b => b.Chapters)).ToList().Select(c => DTOMapper.Map<CategoryShowModel>(c));
            ShlefData = new ObservableCollection<CategoryShowModel>(data);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
            }
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if (View is ShellView view)
            {
                menuHandler = new MenuHandler();
                view.WebBrowser.MenuHandler = menuHandler;
                container.Get<CallbackObjectForJs>().DataContext = this;
            }
        }
    }
}
