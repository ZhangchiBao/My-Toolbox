using AutoMapper;
using Book.Common;
using Book.Models;
using HtmlAgilityPack;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly DBContext db;

        public ShellViewModel(IViewManager viewManager, IContainer container, DBContext db)
        {
            this.container = container;
            this.viewManager = viewManager;
            this.db = db;
            MenuItemCommand = new Command<Action>(a => a?.Invoke());
            LoadShelf();
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

        public ObservableCollection<BookInfo> LocalBooks { get; set; }

        public BookInfo SelectedBook { get; set; }

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
            var list = LocalSearch(SearchKeyword);
            if (list != null && list.Count > 0)
            {
                RadWindow.Confirm(new DialogParameters
                {
                    OkButtonContent = "继续",
                    CancelButtonContent = "取消",
                    Header = "提示",
                    Content = "在本地书架中已找到书籍，是否需要进行在线搜索",
                    Closed = (s, e) =>
                    {
                        if (e.DialogResult ?? false)
                        {
                            SearchOnline();
                        }
                    }
                });
            }
            else
            {
                SearchOnline();
            }
        }

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
        /// 搜索本地书架
        /// </summary>
        /// <returns></returns>
        private IList<TB_Book> LocalSearch(string searchKeyword)
        {
            return db.Books.Where(a => string.IsNullOrEmpty(searchKeyword) || a.Name.Contains(searchKeyword)).ToList();
        }

        public void LoadShelf()
        {
            LocalBooks = new ObservableCollection<BookInfo>(db.Books.ToList().Select(a => Mapper.Map<BookInfo>(a)));
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
                    Task.Run(() =>
                    {
                        UpdateBook(SelectedBook);
                    });
                    break;
            }
        }

        private void UpdateBook(BookInfo selectedBook)
        {
            HtmlWeb web = new HtmlWeb();
            var site = db.Sites.Single(a => a.ID == selectedBook.CurrentSiteID);
            var doc = web.Load(selectedBook.CurrentSource);
            var chapterNodes = doc.DocumentNode.SelectNodes(@"//" + site.ChapterNode);
            foreach (var chapterNode in chapterNodes)
            {
                var chapterName = chapterNode.SelectSingleNode(site.ChapterNameNode)?.InnerText;
                var chapter = db.Chapters.SingleOrDefault(a => a.BookID == selectedBook.ID && a.Title == chapterName);
                var url = chapterNode.SelectSingleNode(site.ChapterUrlNode).GetAttributeValue("href", string.Empty);
                url = WebHelper.Combine(new Uri(selectedBook.CurrentSource), url);
                if (chapter == null)
                {
                    db.Chapters.Add(new TB_Chapter
                    {
                        BookID = selectedBook.ID,
                        Title = chapterName,
                        CurrentSRC = url
                    });
                    db.SaveChanges();
                }
            }
        }
    }
}
