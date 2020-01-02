using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ResourceSearcher.UILogic.Models;

namespace ResourceSearcher.UILogic.Searchers
{
    public class Zhima998Searcher : HttpCommon, ISearcher
    {
        public SearcherData SearchData => new SearcherData("CABAGE");

        public List<ResourceEntity> GetData(string keyword)
        {
            var url = $"https://zhima998.com/infolist.php?q={keyword}";
            string html = GetHtml(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var root = document.DocumentNode;
            var dataNodes = root.SelectNodes("/html/body/div[2]/ul/li");
            return dataNodes.Select(node =>
            {
                var name = node?.ChildNodes?.FirstOrDefault(n => n.Name == "#text")?.InnerText;
                var link = node.SelectSingleNode("a[2]")?.GetAttributeValue("href", string.Empty);
                return new ResourceEntity { Name = name, Link = link, IsMagnetURI = true };
            }).Where(a => !string.IsNullOrEmpty(a.Link)).ToList();
        }
    }
}
