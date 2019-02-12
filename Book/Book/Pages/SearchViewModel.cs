using Book.Models;
using HtmlAgilityPack;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public string SearchKeyword { get; set; }

        public ObservableCollection<BookSearchResult> BookSearchResults { get; set; }

        public bool CanDoSearch => !string.IsNullOrEmpty(SearchKeyword);

        public async void CloseWindow()
        {
            var dialog = (CustomDialog)View;
            await ((MetroWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>())).HideMetroDialogAsync(dialog);
            isOpen = false;
        }

        public void DoSearch()
        {
            isOpen = true;
            BookSearchResults = new ObservableCollection<BookSearchResult>();
            XElement xElement = XElement.Load("sites.xml");
            var nodes = xElement.Nodes();
            var sites = nodes.Select(xNode => JsonConvert.DeserializeObject<SiteInfo>(JsonConvert.SerializeXNode(xNode, Formatting.None, true))).ToList();
            Parallel.ForEach(sites, site =>
            {
                Task.Run(() =>
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
                                var author = resultNode.SelectSingleNode(site.AuthorNode).InnerText;
                                var href = resultNode.SelectSingleNode(site.BookURLNode).GetAttributeValue("href", string.Empty);
                                var update = resultNode.SelectSingleNode(site.UpdateNode).InnerText;
                                var description = resultNode.SelectSingleNode(site.DescriptionNode).InnerText;
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    BookSearchResults.Add(new BookSearchResult { BookName = bookName, Author = author, Description = description, Update = update, SRC = href, Source = site.Name });
                                });
                            }
                        }
                        index++;
                    } while (resultNodes != null && resultNodes.Count >= site.SearchSize && isOpen);
                });
            });
        }
    }
}
