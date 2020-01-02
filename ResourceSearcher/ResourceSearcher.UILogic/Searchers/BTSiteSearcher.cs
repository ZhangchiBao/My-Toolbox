using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ResourceSearcher.UILogic.Models;

namespace ResourceSearcher.UILogic.Searchers
{
    public class BTSiteSearcher : HttpCommon, ISearcher
    {
        public SearcherData SearchData => new SearcherData("BTSite");

        public List<ResourceEntity> GetData(string keyword)
        {
            var url = $"http://www.btsite.net/search/{keyword}/";
            string html = GetHtml(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var root = document.DocumentNode;
            var linkNodes = root.SelectNodes("/html/body/div/table/tr/td[1]/div[1]/a");
            var data = new List<ResourceEntity>();
            foreach (var linkNode in linkNodes)
            {
                var name = linkNode.InnerText;
                var link = linkNode.GetAttributeValue("href", string.Empty);
                if (string.IsNullOrWhiteSpace(link))
                {
                    continue;
                }
                data.Add(new ResourceEntity { Name = linkNode.InnerText, Link = GetLink(url: new Uri(new Uri(url), link).ToString()) });
                Task.Delay(300).Wait();
            }
            data = data.Where(a => !string.IsNullOrEmpty(a.Link)).ToList();
            return data;
        }

        private string GetLink(string url)
        {
            var html = GetHtml(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var root = document.DocumentNode;
            var node = root.SelectSingleNode("/html/body/div/table/tr[6]/td/p/a");
            return node.GetAttributeValue("href", string.Empty);
        }
    }
}
