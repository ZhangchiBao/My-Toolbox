using ResourceSearcher.UILogic.Models;
using ResourceSearcher.UILogic.Searchers;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResourceSearcher.UILogic.ViewModels
{
    public class ShellPageViewModel : BaseViewModel
    {
        private readonly IEnumerable<ISearcher> searchers;

        public ShellPageViewModel(IContainer container) : base(container)
        {
            searchers = container.GetAll<ISearcher>();
            Resources = new ObservableCollection<ResourceEntity>();
            CloseTabCommand = new Command<SearcherDataEntity>(CloseTab);
        }

        private void CloseTab(SearcherDataEntity obj)
        {
            Data.Remove(obj);
        }

        public string Keyword { get; set; }

        public ObservableCollection<ResourceEntity> Resources { get; set; }
        public ObservableCollection<SearcherDataEntity> Data { get; set; } = new ObservableCollection<SearcherDataEntity>();
        public Command<SearcherDataEntity> CloseTabCommand { get; private set; }
        public bool Loading { get; set; }

        public void Keyword_KeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(Keyword))
            {
                View.Dispatcher.Invoke(() =>
                {
                    var tabItem = Data.SingleOrDefault(a => a.Keyword == Keyword);
                    if (tabItem != null)
                    {
                        tabItem.IsSelected = true;
                    }
                    else
                    {
                        tabItem = new SearcherDataEntity(Keyword);
                        Data.Add(tabItem);
                        foreach (var searcher in searchers)
                        {
                            var tabSearcher = new Searcher(searcher.SearchData);
                            tabItem.Searchers.Add(tabSearcher);
                            Task.Run(() =>
                            {
                                View.Dispatcher.Invoke(() =>
                                {
                                    tabSearcher.Loading = true;
                                });
                                try
                                {
                                    List<ResourceEntity> items = searcher.GetData(Keyword);
                                    View.Dispatcher.Invoke(() =>
                                    {
                                        foreach (var item in items)
                                        {
                                            tabSearcher.Resources.Add(item);
                                        }
                                    });
                                }
                                catch (Exception)
                                {

                                }
                                View.Dispatcher.Invoke(() =>
                                {
                                    tabSearcher.Loading = false;
                                });
                            });
                        }
                    }
                });
            }
        }
    }
}
