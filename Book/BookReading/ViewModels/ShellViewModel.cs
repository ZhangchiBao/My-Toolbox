using BookReading.Entities;
using BookReading.Libs;
using BookReading.Views;
using Newtonsoft.Json;
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

        #region 公共属性
        /// <summary>
        /// 书架
        /// </summary>
        public ObservableCollection<CategoryShowModel> ShlefData { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 当前小说
        /// </summary>
        private BookShowModel currentBook;

        public string ShowFile { get; set; }

        /// <summary>
        /// 主窗体内容
        /// </summary>
        public object MainContentObject { get; set; }

        /// <summary>
        /// 能否前往上一章
        /// </summary>
        public bool CanGotoPreviousChapter { get; set; }

        /// <summary>
        /// 能否前往下一章
        /// </summary>
        public bool CanGotoNextChapter { get; set; }
        #endregion

        #region 公共方法
        /// <summary>
        /// 前往上一章
        /// </summary>
        /// <param name="chapter"></param>
        public async void GotoPreviousChapterAsync(ChapterShowModel chapter)
        {
            await GotoChapterAsync(currentBook.Chapters.Single(a => a.Index == (chapter.Index - 1)));
        }

        /// <summary>
        /// 前往下一章
        /// </summary>
        /// <param name="chapter"></param>
        public async void GotoNextChapterAsync(ChapterShowModel chapter)
        {
            await GotoChapterAsync(currentBook.Chapters.Single(a => a.Index == (chapter.Index + 1)));
        }

        /// <summary>
        /// 前往目录
        /// </summary>
        public void GotoCatalog()
        {
            MainContentObject = currentBook;
        }

        /// <summary>
        /// 弹出搜索窗口
        /// </summary>
        public void ShowSearchView()
        {
            var vm = container.Get<SearchViewModel>();
            vm.Keyword = Keyword;
            if (!string.IsNullOrWhiteSpace(vm.Keyword))
            {
                vm.ViewLoaded += vm.DoSearch;
            }
            if (windowManager.ShowDialog(vm) ?? false)
            {
                Task.Run(LoadBookShelf);
            }
        }

        /// <summary>
        /// 搜索框按下按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 目录中点击章节
        /// </summary>
        /// <param name="chapter"></param>
        public async void ChapterClickAsync(ChapterShowModel chapter)
        {
            await GotoChapterAsync(chapter);
        }

        /// <summary>
        /// 更新目录
        /// </summary>
        /// <param name="category"></param>
        public void UpdateCategory(CategoryShowModel category)
        {
            foreach (var book in category.Books)
            {
                UpdateBookAsync(book);
            }
        }

        /// <summary>
        /// 更新小说
        /// </summary>
        /// <param name="book"></param>
        public async void UpdateBookAsync(BookShowModel book)
        {
            var finders = container.GetAll<IFinder>();
            var finder = finders.Single(a => a.FinderKey == book.FinderKey);
            await UpdateAllChaptersAsync(book, finder);
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
        /// 下载所有未下载章节内容
        /// </summary>
        /// <param name="book"></param>
        public async void DownloadAllUndownloadedChaptersAsync(BookShowModel book)
        {
            foreach (var chapter in book.Chapters)
            {
                if (chapter.Downloaded)
                {
                    continue;
                }

                await DownloadChapterContentAsync(chapter);
            }
            base.ShowMessage("所有章节内容下载成功");
        }
        #endregion

        /// <summary>
        /// 切换章节
        /// </summary>
        /// <param name="chapter"></param>
        private async Task GotoChapterAsync(ChapterShowModel chapter)
        {
            if (chapter.Sections == null || chapter.Sections.Count == 0)
            {
                await DownloadChapterContentAsync(chapter);
            }
            MainContentObject = chapter;
            CanGotoPreviousChapter = chapter.Index > 0;
            CanGotoNextChapter = chapter.Index < currentBook.Chapters.Max(a => a.Index);
        }

        /// <summary>
        /// 下载章节内容
        /// </summary>
        /// <param name="chapter"></param>
        private async Task DownloadChapterContentAsync(ChapterShowModel chapter)
        {
            try
            {
                var finder = container.GetAll<IFinder>().Single(a => a.FinderKey == chapter.FinderKey);
                chapter.Sections = new ObservableCollection<string>(await finder.GetParagraphListAsync(chapter.URL));
                var dbChapter = await db.Chapters.SingleAsync(a => a.ID == chapter.ID.ToString());
                dbChapter.Sections = JsonConvert.SerializeObject(chapter.Sections);
                dbChapter.Downloaded = true;
                await db.SaveChangesAsync();
            }
            catch
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

        /// <summary>
        /// 更新所有章节
        /// </summary>
        /// <param name="book"></param>
        /// <param name="finder"></param>
        /// <returns></returns>
        private async Task UpdateAllChaptersAsync(BookShowModel book, IFinder finder)
        {
            var chapterList = await finder.GetChaptersAsync(book.URL);
            var db = container.Get<BookContext>();
            for (int i = 0; i < chapterList.Count; i++)
            {
                var chapter = chapterList[i];
                if (db.Chapters.Any(a => a.BookID == book.ID.ToString() && a.Title == chapter.Title))
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
                await db.SaveChangesAsync();
                book.Chapters = new ObservableCollection<ChapterShowModel>(db.Chapters.Where(a => a.BookID == book.ID.ToString()).Select(a => DTOMapper.Map<ChapterShowModel>(a)));
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(MainContentObject):
                    ((ShellView)View).MainContentScrollViewer.ScrollToVerticalOffset(0);
                    break;
            }
        }
    }
}
