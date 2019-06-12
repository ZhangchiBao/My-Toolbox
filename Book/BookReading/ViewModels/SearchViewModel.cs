using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookReading.Entities;
using BookReading.Libs;
using BookReading.Libs.Entity;
using System.Data.Entity;
using Stylet;
using StyletIoC;
using System.Net.Http;

namespace BookReading.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager, BookContext db) : base(container, windowManager, viewManager, db)
        {
            FinderCollection = new ObservableCollection<FinderStatusModel>(container.GetAll<IFinder>().Select(finder => new FinderStatusModel(finder)));
        }

        #region 公开属性
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 能否执行搜索
        /// </summary>
        public bool CanDoSearch => !string.IsNullOrWhiteSpace(Keyword);

        /// <summary>
        /// 能否执行下载
        /// </summary>
        public bool CanDownload => SelectedSearchResult != null;

        public ObservableCollection<FinderStatusModel> FinderCollection { get; }

        public ObservableCollection<BookModel> SearchResultData { get; set; }

        public BookModel SelectedSearchResult { get; set; }
        #endregion

        #region 公共方法
        /// <summary>
        /// 执行搜索
        /// </summary>
        public void DoSearch()
        {
            SearchResultData = new ObservableCollection<BookModel>();
            foreach (var finder in FinderCollection)
            {
                Task.Run(() =>
                {
                    ExecuteOnView(async () =>
                    {
                        finder.DoneStatus = DoneStatus.Doing;
                        var result = await finder.Finder.SearchByKeywordAsync(Keyword);
                        foreach (var item in result)
                        {
                            SearchResultData.Add(item);
                        }
                        ExecuteOnView(() =>
                        {
                            finder.DoneStatus = DoneStatus.Done;
                        });
                    });
                });
            }
        }

        public void Keyword_Inputbox_Keydown()
        {
            if (CanDoSearch)
            {
                DoSearch();
            }
        }

        public void ResultView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is System.Windows.FrameworkElement element && element.DataContext is BookModel book)
            {
                SelectedSearchResult = book;
                Download();
            }
        }

        /// <summary>
        /// 执行退出
        /// </summary>
        public void Exist()
        {
            base.RequestClose();
        }

        /// <summary>
        /// 执行下载
        /// </summary>
        public async void Download()
        {
            if (db.Books.Any(a => a.Name == SelectedSearchResult.BookName && a.Author == SelectedSearchResult.Author))
            {
                MessageBox.Show("书架中已经存在同样的小说！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var category = db.Categories.Include(c => c.Alias).FirstOrDefault(a => a.Alias.Any(b => b.AliasName == SelectedSearchResult.Category));
            if (category == null)
            {
                category = db.Categories.Single(a => a.CategoryName == "其他");
            }
            var bookFloder = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{SelectedSearchResult.BookName}_{SelectedSearchResult.Author}")).Replace(@"\", "").Replace(".", "").Replace("/", "");
            var chapterList = await SelectedSearchResult.Finder.GetChaptersAsync(SelectedSearchResult.URL);
            db.Books.Add(new Book
            {
                ID = Guid.NewGuid().ToString(),
                Author = SelectedSearchResult.Author,
                Name = SelectedSearchResult.BookName,
                CoverURL = SelectedSearchResult.Cover,
                Description = SelectedSearchResult.Description,
                FinderKey = SelectedSearchResult.Finder.FinderKey.ToString(),
                URL = SelectedSearchResult.URL,
                CategoryID = category.ID,
                Chapters = chapterList.Select((c, i) => new Chapter
                {
                    Downloaded = false,
                    FinderKey = SelectedSearchResult.Finder.FinderKey.ToString(),
                    ID = Guid.NewGuid().ToString(),
                    Index = i,
                    Title = c.Title,
                    URL = c.URL
                }).ToList()
            });
            db.SaveChanges();
            RequestClose(true);
        }

        private async Task<string> GetCoverContentAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var buffer = await client.GetByteArrayAsync(url);
                return Convert.ToBase64String(buffer);
            }
        }
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedSearchResult):
                    NotifyOfPropertyChange(nameof(CanDownload));
                    break;
            }
        }
    }
}
