using ResourceSearcher.UILogic.Searchers;
using Stylet;
using System;
using System.Collections.ObjectModel;

namespace ResourceSearcher.UILogic.Models
{
    public class SearcherDataEntity : PropertyChangedBase
    {
        public SearcherDataEntity(string keyword)
        {
            Keyword = keyword;
            IsSelected = true;
            Searchers = new ObservableCollection<Searcher>();
        }
        public string Keyword { get; }
        public bool IsSelected { get; set; }
        public ObservableCollection<Searcher> Searchers { get; set; }
    }

    public class Searcher : PropertyChangedBase
    {
        public Searcher(SearcherData searcherData)
        {
            SearcherData = searcherData ?? throw new ArgumentNullException(nameof(searcherData));
            Resources = new ObservableCollection<ResourceEntity>();
        }

        public SearcherData SearcherData { get; set; }

        public ObservableCollection<ResourceEntity> Resources { get; set; }
        public bool Loading { get; set; }
    }
}
