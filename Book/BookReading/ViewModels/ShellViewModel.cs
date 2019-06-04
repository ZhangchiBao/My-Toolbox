using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookReading.Entities;
using BookReading.MenuHandlers;
using BookReading.Views;
using Stylet;
using StyletIoC;

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
            if (e.OriginalSource is System.Windows.FrameworkElement element && element.DataContext is BookShowModel book)
            {
                ShowFile = Path.Combine(App.BOOKSHELF_FLODER, book.BookFloder, "List.htm");
                menuHandler.MenuItems.Add("测试菜单1", null);
                menuHandler.MenuItems.Add("测试菜单2", null);
            }
            else
            {
            }
        }

        /// <summary>
        /// 加载书架
        /// </summary>
        private void LoadBookShelf()
        {
            var data = db.Categories.Include(a => a.Books).Include(a => a.Books.Select(b => b.Chapters)).ToList().Select(category => new CategoryShowModel
            {
                ID = new Guid(category.ID),
                Name = category.CategoryName,
                Books = new ObservableCollection<BookShowModel>(category.Books.Select(book => new BookShowModel
                {
                    ID = new Guid(book.ID),
                    Name = book.BookName,
                    Author = book.Author,
                    Descption = book.Descption,
                    Cover = book.CoverURL,
                    FinderKey = new Guid(book.FinderKey),
                    BookFloder = book.BookFloder,
                    Chapters = new ObservableCollection<ChapterShowModel>(book.Chapters.Select((chapter, index) => new ChapterShowModel
                    {
                        Index = index,
                        ID = new Guid(chapter.ID),
                        Title = chapter.Title,
                        Downloaded = chapter.Downloaded,
                        FilePath = chapter.FilePath,
                        FinderKey = new Guid(chapter.FinderKey)
                    }))
                }))
            });
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
                menuHandler = new MenuHandlers.MenuHandler();
                view.WebBrowser.MenuHandler = menuHandler;
            }
        }
    }
}
