using ResourceSearcher.UILogic.Models;
using System.Collections.Generic;

namespace ResourceSearcher.UILogic.Searchers
{
    public interface ISearcher
    {
        SearcherData SearchData { get; }

        List<ResourceEntity> GetData(string keyword);
    }

    public class SearcherData
    {
        public SearcherData(string searchName)
        {
            SearchName = searchName;
        }

        public string SearchName { get; }
    }
}