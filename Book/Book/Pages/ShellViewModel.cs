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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly SitesDBContext db;

        public ShellViewModel(IViewManager viewManager, IContainer container, SitesDBContext db)
        {
            this.container = container;
            this.viewManager = viewManager;
            this.db = db;
            MenuItemCommand = new Command<Action>(a => a?.Invoke());
            RefreshShelf();
        }

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


        public bool IsBusy { get; set; }

        /// <summary>
        /// 当前选中书籍
        /// </summary>
        public BookInfo SelectedBook { get; set; }

        public bool CanDeleteBook => SelectedBook != null;

        public bool CanUpdateBook => SelectedBook != null;
        public BookShowViewModel BookShowViewModel { get; set; }

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
        public void RefreshShelf()
        {
            LocalBooks = new ObservableCollection<BookInfo>(db.Books.ToList().Select(a => Mapper.Map<BookInfo>(a)));
        }

        /// <summary>
        /// 删除小说
        /// </summary>
        public void DeleteBook()
        {
            if (BookShowViewModel.CurrentBook == SelectedBook)
            {
                BookShowViewModel.CurrentBook = null;
            }

            db.Books.RemoveRange(db.Books.Where(a => a.ID == SelectedBook.ID));
            db.SaveChanges();
            File.Delete($"{App.SHELF_DIRECTORY}//{SelectedBook.Name}.db");
            RefreshShelf();
        }

        public void UpdateBook()
        {
            Task.Run(() =>
            {
                IsBusy = true;
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
                BookShowViewModel.CurrentBook = null;
                BookShowViewModel.CurrentBook = book;
            }).ContinueWith(task =>
            {
                IsBusy = false;
            });
        }

        public void BookDoubleClick(object sender, RadRoutedEventArgs e)
        {
            if (((dynamic)e.OriginalSource).DataContext is BookInfo book)
            {
                BookShowViewModel.CurrentBook = null;
                BookShowViewModel.CurrentBook = book;
            }
        }

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
    }
}
