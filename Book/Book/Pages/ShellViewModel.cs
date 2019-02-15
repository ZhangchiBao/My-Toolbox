using AutoMapper;
using Book.Common;
using Book.Models;
using HtmlAgilityPack;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class ShellViewModel : Screen
    {
        #region 私有字段
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly SitesDBContext db;
        private readonly Timer timer;
        #endregion

        public ShellViewModel(IViewManager viewManager, IContainer container, SitesDBContext db)
        {
            this.container = container;
            this.viewManager = viewManager;
            this.db = db;
            MenuItemCommand = new Command<Action>(a => a?.Invoke());
            timer = new Timer(1000);
            timer.Elapsed += (s, e) =>
            {
                var regex = new Regex(@"[·]+$");
                var match = regex.Match(BusyContent);
                if (match.Success)
                {
                    var length = match.Value.Length;
                    if (length > 3)
                    {
                        BusyContent = BusyContent.TrimEnd('·');
                    }
                    else
                    {
                        BusyContent = BusyContent + "·";
                    }
                }
                else
                {
                    BusyContent = BusyContent.TrimEnd('·') + "·";
                }
            };
            Task.Run(async () =>
            {
                await RefreshShelf();
            });
        }

        #region 公共属性
        /// <summary>
        /// 站点设置
        /// </summary>
        public void SitesSetting()
        {
            container.Get<SitesSettingViewModel>().LoadSites();
            var sitesSettingView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<SitesSettingViewModel>());
            sitesSettingView.Owner = (Window)View;
            sitesSettingView.ShowDialog();
        }

        /// <summary>
        /// 菜单操作命令
        /// </summary>
        public ICommand MenuItemCommand { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword { get; set; }

        /// <summary>
        /// 能否执行搜索
        /// </summary>
        public bool CanDoSearch => !string.IsNullOrEmpty(SearchKeyword);

        /// <summary>
        /// 本地书架
        /// </summary>
        public ObservableCollection<BookInfo> LocalBooks { get; set; }

        /// <summary>
        /// 任务正在忙
        /// </summary>
        public bool IsBusy { get; set; }

        /// <summary>
        /// 任务忙提示
        /// </summary>
        public string BusyContent { get; set; }

        /// <summary>
        /// 当前选中书籍
        /// </summary>
        public BookInfo SelectedBook { get; set; }

        /// <summary>
        /// 能否删除小说
        /// </summary>
        public bool CanDeleteBook => SelectedBook != null;

        /// <summary>
        /// 能否更新小说
        /// </summary>
        public bool CanUpdateBook => SelectedBook != null;

        /// <summary>
        /// 能否制作电子书
        /// </summary>
        public bool CanExportEBook => SelectedBook != null;

        /// <summary>
        /// 小说展示VM
        /// </summary>
        public BookShowViewModel BookShowViewModel { get; set; }
        #endregion

        #region 公共方法
        /// <summary>
        /// 搜索输入框按键被按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SearchBoxKeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CanDoSearch)
            {
                DoSearch();
            }
        }

        /// <summary>
        /// 执行搜索
        /// </summary>
        public void DoSearch()
        {
            SearchOnline();
        }

        /// <summary>
        /// 在线搜索
        /// </summary>
        private void SearchOnline()
        {
            var searchViewModel = container.Get<SearchViewModel>();
            searchViewModel.SearchKeyword = SearchKeyword;
            var searchView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(searchViewModel);
            searchView.Owner = (Window)View;
            searchViewModel.DoSearch();
            searchView.ShowDialog();
        }

        /// <summary>
        /// 刷新书架
        /// </summary>
        public async Task RefreshShelf()
        {
            await StartBusy(() =>
            {
                LocalBooks = new ObservableCollection<BookInfo>(db.Books.ToList().Select(a => Mapper.Map<BookInfo>(a)));
            }, "正在更新书架");
        }

        /// <summary>
        /// 删除小说
        /// </summary>
        public async void DeleteBook()
        {
            await StartBusy(() =>
            {
                if (BookShowViewModel.CurrentBook == SelectedBook)
                {
                    BookShowViewModel.CurrentBook = null;
                }

                db.Books.RemoveRange(db.Books.Where(a => a.ID == SelectedBook.ID));
                db.SaveChanges();
                File.Delete($"{App.SHELF_DIRECTORY}//{SelectedBook.Name}.db");
            }, $"正在删除小说{SelectedBook.Name}");
            await RefreshShelf();
        }

        /// <summary>
        /// 更新小说
        /// </summary>
        public async void UpdateBook()
        {
            await StartBusy(() =>
            {
                HtmlWeb web = new HtmlWeb();
                var site = db.Sites.Single(a => a.ID == SelectedBook.CurrentSiteID);
                var doc = web.Load(SelectedBook.CurrentSource);
                var chapterNodes = doc.DocumentNode.SelectNodes(@"//" + site.ChapterNode);
                using (var bookDB = new BookDBContext(SelectedBook.Name))
                {
                    foreach (var chapterNode in chapterNodes)
                    {
                        var chapterName = chapterNode.SelectSingleNode(site.ChapterNameNode)?.InnerText;
                        var chapter = bookDB.Chapters.SingleOrDefault(a => a.BookID == SelectedBook.ID && a.Title == chapterName);
                        var url = chapterNode.SelectSingleNode(site.ChapterUrlNode).GetAttributeValue("href", string.Empty);
                        url = WebHelper.Combine(new Uri(SelectedBook.CurrentSource), url);
                        if (chapter == null)
                        {
                            bookDB.Chapters.Add(new TB_Chapter
                            {
                                BookID = SelectedBook.ID,
                                Title = chapterName,
                                CurrentSRC = url,
                                SiteID = site.ID
                            });
                            bookDB.SaveChanges();
                        }
                    }
                }
                var book = Mapper.Map<BookInfo>(db.Books.SingleOrDefault(a => a.ID == SelectedBook.ID));
                if (BookShowViewModel.CurrentBook.Name == book.Name)
                {
                    BookShowViewModel.RefreshChapterList();
                }
            }, "正在更新");
        }

        /// <summary>
        /// 制作电子书
        /// </summary>
        public void ExportEBook()
        {
            ExportViewModel exportViewModel = container.Get<ExportViewModel>();
            exportViewModel.CurrentBook = SelectedBook;
            RadWindow exportView = ((RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(exportViewModel));
            exportView.Owner = (Window)View;
            exportView.ShowDialog();
        }

        /// <summary>
        /// 下载所有空白章节
        /// </summary>
        public async void DownloadAllNoContentChapter()
        {
            await StartBusy(() =>
            {
                HtmlWeb web = new HtmlWeb();
                var site = db.Sites.Single(a => a.ID == SelectedBook.CurrentSiteID);
                var doc = web.Load(SelectedBook.CurrentSource);
                var chapterNodes = doc.DocumentNode.SelectNodes(@"//" + site.ChapterNode);
                using (var bookDB = new BookDBContext(SelectedBook.Name))
                {
                    foreach (var chapterNode in chapterNodes)
                    {
                        var chapterName = chapterNode.SelectSingleNode(site.ChapterNameNode)?.InnerText;
                        var chapter = bookDB.Chapters.SingleOrDefault(a => a.BookID == SelectedBook.ID && a.Title == chapterName);
                        var url = chapterNode.SelectSingleNode(site.ChapterUrlNode).GetAttributeValue("href", string.Empty);
                        url = WebHelper.Combine(new Uri(SelectedBook.CurrentSource), url);
                        if (chapter != null && string.IsNullOrEmpty(chapter.Content))
                        {
                            var contentDoc = web.Load(url);
                            var content = contentDoc.DocumentNode.SelectSingleNode(@"//" + site.ContentNode).InnerText;
                            content = "　　" + string.Join("\r\n　　", content.Split('\r', '\n').Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().Replace("　　", "\r\n　　")));
                            chapter.Content = content;
                            bookDB.SaveChanges();
                        }
                        else if (chapter == null)
                        {
                            var contentDoc = web.Load(url);
                            var content = contentDoc.DocumentNode.SelectSingleNode(@"//" + site.ContentNode).InnerText;
                            content = "　　" + string.Join("\r\n　　", content.Split('\r', '\n').Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().Replace("　　", "\r\n　　")));
                            bookDB.Chapters.Add(new TB_Chapter
                            {
                                BookID = SelectedBook.ID,
                                Title = chapterName,
                                CurrentSRC = url,
                                SiteID = site.ID,
                                Content = content
                            });
                            bookDB.SaveChanges();
                        }
                    }
                }
                var book = Mapper.Map<BookInfo>(db.Books.SingleOrDefault(a => a.ID == SelectedBook.ID));
                if (BookShowViewModel.CurrentBook.Name == book.Name)
                {
                    BookShowViewModel.RefreshChapterList();
                }
            }, "正在下载所有空白章节");
        }

        /// <summary>
        /// 书架双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BookDoubleClick(object sender, RadRoutedEventArgs e)
        {
            if (((dynamic)e.OriginalSource).DataContext is BookInfo book)
            {
                BookShowViewModel.CurrentBook = null;
                BookShowViewModel.CurrentBook = book;
            }
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="content"></param>
        public async Task StartBusy(Action action, string content = null)
        {
            await Task.Run(async () =>
            {
                BusyContent = content ?? "正在加载";
                IsBusy = true;
                timer.Start();
                await Task.Delay(500);
                action.Invoke();
            }).ContinueWith(task =>
            {
                timer.Stop();
                IsBusy = false;
                BusyContent = null;
            });
        }

        /// <summary>
        /// 书架右键菜单展开时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BookShelfContextMenuOpened(object sender, RoutedEventArgs e)
        {
            if (sender is RadContextMenu menu)
            {
                RadTreeViewItem clickedItemContainer = menu.GetClickedElement<RadTreeViewItem>();
                if (clickedItemContainer != null)
                {
                    SelectedBook = clickedItemContainer.Item as BookInfo;
                }
                else
                {
                    SelectedBook = null;
                }
            }
        }
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SearchKeyword):
                    NotifyOfPropertyChange(nameof(CanDoSearch));
                    break;
                case nameof(SelectedBook):
                    NotifyOfPropertyChange(nameof(CanDeleteBook));
                    break;
            }
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            BookShowViewModel = container.Get<BookShowViewModel>();
        }
    }
}
