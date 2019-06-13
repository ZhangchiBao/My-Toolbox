using BookReading.Entities;
using BookReading.Libs;
using BookReading.Views;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookReading.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
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

        public object MainContentObject { get; set; }

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

        public void ChapterClick(ChapterShowModel chapter)
        {

        }

        public void UpdateCategory(CategoryShowModel category)
        {
            foreach (var book in category.Books)
            {
                UpdateBookAsync(book);
            }
        }

        public async void UpdateBookAsync(BookShowModel book)
        {
            var finders = container.GetAll<IFinder>();
            var finder = finders.Single(a => a.FinderKey == book.FinderKey);
            var chapterList = await finder.GetChaptersAsync(book.URL);
            var db = container.Get<BookContext>();
            for (int i = 0; i < chapterList.Count; i++)
            {
                if (db.Chapters.Any(a => a.BookID == book.ID.ToString() && a.Title == chapterList[i].Title))
                {
                    continue;
                }
                db.Chapters.Add(new Chapter
                {
                    Title = chapterList[i].Title,
                    BookID = book.ID.ToString(),
                    ID = Guid.NewGuid().ToString(),
                    Downloaded = false,
                    FinderKey = finder.FinderKey.ToString(),
                    Index = i,
                    URL = chapterList[i].URL
                });
                db.SaveChanges();
            }
            LoadBookShelf();
        }

        /// <summary>
        /// 书架双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Shelf_DoubleClickAsync(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is System.Windows.FrameworkElement element && element.DataContext is BookShowModel book)
            {
                currentBook = book;
                if (book.CoverContent == null || book.CoverContent.Length == 0)
                {
                    await DownloadCoverAsync(book);
                }
                MainContentObject = book;
            }
            else
            {
            }
        }

        /// <summary>
        /// 下载封面
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private async Task DownloadCoverAsync(BookShowModel book)
        {
            using (var client = new HttpClient())
            {
                if (string.IsNullOrEmpty(book.CoverUrl))
                {
                    return;
                }

                var buffer = await client.GetByteArrayAsync(book.CoverUrl);
                var coverContent = Convert.ToBase64String(buffer);
                ExecuteOnView(() =>
                {
                    book.CoverContent = buffer;
                });
                var db = container.Get<BookContext>();
                var item = db.Books.Single(a => a.ID == book.ID.ToString());
                item.CoverContent = coverContent;
                db.SaveChanges();
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
            }
        }
    }
}
