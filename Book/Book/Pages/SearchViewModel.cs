using AutoMapper;
using Book.Common;
using Book.Models;
using HtmlAgilityPack;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class SearchViewModel : Screen
    {
        private readonly IViewManager viewManager;
        private readonly IContainer container;
        private readonly SitesDBContext db;
        private bool isOpen;

        public SearchViewModel(IViewManager viewManager, IContainer container, SitesDBContext db)
        {
            this.viewManager = viewManager;
            this.container = container;
            this.db = db;
        }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword { get; set; }

        /// <summary>
        /// 选中搜索结果
        /// </summary>
        public BookSearchResult SelectedBookSearchResult { get; set; }

        /// <summary>
        /// 搜索结果
        /// </summary>
        public ObservableCollection<BookSearchResult> BookSearchResults { get; set; }

        /// <summary>
        /// 能否执行搜索
        /// </summary>
        public bool CanDoSearch => !string.IsNullOrEmpty(SearchKeyword);

        /// <summary>
        /// 关闭弹窗
        /// </summary>
        public void CloseDialog()
        {
            var dialog = (RadWindow)View;
            isOpen = false;
            dialog.Close();
        }

        /// <summary>
        /// 执行搜索
        /// </summary>
        public void DoSearch()
        {
            isOpen = false;
            BookSearchResults = new ObservableCollection<BookSearchResult>();
            var sites = db.Sites.Select(a => Mapper.Map<SiteInfo>(a)).ToList();
            var tasks = sites.Select(site => new Task(() =>
            {
                HtmlNodeCollection resultNodes = null;
                int index = 0;
                do
                {
                    HtmlWeb web = new HtmlWeb();
                    var url = site.SearchURL.Replace("[s]", SearchKeyword).Replace("[p]", index.ToString());
                    var doc = web.Load(url);
                    resultNodes = doc.DocumentNode.SelectNodes(@"//" + site.BookResultsNode);
                    if (resultNodes != null)
                    {
                        foreach (HtmlNode resultNode in resultNodes)
                        {
                            var bookName = resultNode.SelectSingleNode(site.BookNameNode).InnerText;
                            var author = resultNode.SelectSingleNode(site.AuthorNode)?.InnerText;
                            var href = resultNode.SelectSingleNode(site.BookURLNode).GetAttributeValue("href", string.Empty);
                            if (new Uri(url).Host.Contains("qidian.com"))
                            {
                                href += "#Catalog";
                            }
                            string update = string.Empty;
                            if (!string.IsNullOrEmpty(site.UpdateNode))
                            {
                                update = resultNode.SelectSingleNode(site.UpdateNode)?.InnerText;
                            }
                            string description = string.Empty;
                            if (!string.IsNullOrEmpty(site.DescriptionNode))
                            {
                                description = resultNode.SelectSingleNode(site.DescriptionNode)?.InnerText;
                            }

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                BookSearchResults.Add(new BookSearchResult
                                {
                                    BookName = bookName?.Trim('\r')?.Trim('\n')?.Trim(),
                                    Author = author?.Trim('\r')?.Trim('\n')?.Trim(),
                                    Description = description?.Trim('\r')?.Trim('\n')?.Trim(),
                                    Update = update?.Trim('\r')?.Trim('\n')?.Trim(),
                                    SRC = WebHelper.Combine(new Uri(url), href),
                                    Source = site.Name?.Trim('\r')?.Trim('\n')?.Trim(),
                                    SiteID = site.ID
                                });
                            });
                        }
                    }
                    index++;
                } while (resultNodes != null && resultNodes.Count >= site.SearchSize && isOpen);
            })).ToList();
            isOpen = true;
            tasks.ForEach(task => task.Start());
        }

        /// <summary>
        /// 确认选择
        /// </summary>
        public void ConfirmSelect()
        {
            if (SelectedBookSearchResult != null)
            {
                RadWindow.Confirm(new DialogParameters
                {
                    Header = "提示",
                    Content = $"确定要添加《{SelectedBookSearchResult.BookName}》到书架么？",
                    OkButtonContent = "确定",
                    CancelButtonContent = "取消",
                    Closed = (o, arg) =>
                    {
                        if (arg.DialogResult ?? false)
                        {
                            SaveToShelf(SelectedBookSearchResult);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 保存到书架
        /// </summary>
        /// <param name="bookSearchResult"></param>
        private void SaveToShelf(BookSearchResult bookSearchResult)
        {
            using (var bookDB = new BookDBContext(bookSearchResult.BookName))
            {
                var book = db.Books.SingleOrDefault(a => a.Name == bookSearchResult.BookName);
                if (book == null)
                {
                    book = new TB_Book
                    {
                        Name = bookSearchResult.BookName,
                        Author = bookSearchResult.Author,
                        Description = bookSearchResult.Description,
                        Type = string.Empty
                    };
                    db.Books.Add(book);
                    db.SaveChanges();
                }
                book.CurrentSource = bookSearchResult.SRC;
                book.CurrentSiteID = bookSearchResult.SiteID;
                db.SaveChanges();
            }
            container.Get<ShellViewModel>().RefreshShelf();
            CloseDialog();
        }

        /// <summary>
        /// 搜索结果列表双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SearchResultsViewDoubleClick(object sender, MouseEventArgs e)
        {
            if (((FrameworkElement)e.OriginalSource).DataContext is BookSearchResult bookSearchResult)
            {
                RadWindow.Confirm(new DialogParameters
                {
                    Header = "提示",
                    Content = $"确定要添加《{bookSearchResult.BookName}》到书架么？",
                    OkButtonContent = "确定",
                    CancelButtonContent = "取消",
                    Closed = (o, arg) =>
                    {
                        if (arg.DialogResult ?? false)
                        {
                            SaveToShelf(bookSearchResult);
                        }
                    }
                });
            }
        }
    }
}
