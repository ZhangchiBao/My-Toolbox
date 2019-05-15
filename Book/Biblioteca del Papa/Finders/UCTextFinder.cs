using Biblioteca_del_Papa.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca_del_Papa.Finders
{
    public class UCTextFinder : IFinder
    {
        public string FinderName => "UC书盟";

        public Guid FinderKey => new Guid("233A5DE7-FB6B-4CC3-A27F-844BD9B1766E");

        public IList<ChapterInfo> GetChapters(string url)
        {
            throw new NotImplementedException();
        }

        public string GetContent(string url)
        {
            throw new NotImplementedException();
        }

        public IList<BookInfo> SearchByKeyword(string keyword)
        {
            var data = new List<BookInfo>();
            var url = $"http://www.uctxt.com/modules/article/search.php?searchkey={System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GBK"))}";
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.GetEncoding("GBK");
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//*[@id='main']/section/div[2]/ul/li");
            foreach (var node in nodes)
            {
                data.Add(new BookInfo(this)
                {
                    BookName = node.SelectSingleNode("span[2]/a").InnerText,
                    Category = node.SelectSingleNode("span[1]").InnerText.Trim('[', ']'),
                    Author = node.SelectSingleNode("span[3]/text()").InnerText,
                    URL = new Uri(new Uri(url), node.SelectSingleNode("span[2]/a").GetAttributeValue("href", string.Empty)).ToString(),
                    Latestchapters = node.SelectSingleNode("span[2]/small/a").InnerText
                });
            }
            return data;
        }
    }
}
