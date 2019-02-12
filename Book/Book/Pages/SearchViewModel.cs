using Book.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class SearchViewModel : Screen
    {
        private readonly IViewManager viewManager;
        private readonly IContainer container;
        private readonly SitesSettingViewModel sitesSettingViewModel;
        private bool isOpen;

        public SearchViewModel(IViewManager viewManager, IContainer container)
        {
            this.viewManager = viewManager;
            this.container = container;
            sitesSettingViewModel = container.Get<SitesSettingViewModel>();
            sitesSettingViewModel.Sites = new ObservableCollection<SiteInfo>();
        }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword { get; set; }

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
        public void CloseWindow()
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
            XElement xElement = XElement.Load("sites.xml");
            var nodes = xElement.Nodes();
            var sites = nodes.Select(xNode => JsonConvert.DeserializeObject<SiteInfo>(JsonConvert.SerializeXNode(xNode, Formatting.None, true))).ToList();
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

                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                BookSearchResults.Add(new BookSearchResult
                                {
                                    BookName = bookName?.Trim('\r')?.Trim('\n')?.Trim(),
                                    Author = author?.Trim('\r')?.Trim('\n')?.Trim(),
                                    Description = description?.Trim('\r')?.Trim('\n')?.Trim(),
                                    Update = update?.Trim('\r')?.Trim('\n')?.Trim(),
                                    SRC = href,
                                    Source = site.Name?.Trim('\r')?.Trim('\n')?.Trim()
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
    }
}
