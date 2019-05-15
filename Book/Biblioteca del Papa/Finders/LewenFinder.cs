using Biblioteca_del_Papa.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Biblioteca_del_Papa.Finders
{
    public class LewenFinder : IFinder
    {
        public string FinderName => "乐文小说网";

        public Guid FinderKey => new Guid("A2D54805-86D4-4729-91B5-29CCFAF2B579");

        public IList<ChapterInfo> GetChapters(string url)
        {
            var data = new List<ChapterInfo>();

            return data;
        }

        public string GetContent(string url)
        {
            throw new NotImplementedException();
        }

        public IList<BookInfo> SearchByKeyword(string keyword)
        {
            var data = new List<BookInfo>();
            var url = $"https://www.lewenxiaoshuo.com/novel.php?action=search";
            HtmlDocument doc = new HtmlDocument();
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsync(url, new StringContent($"searchkey={System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GBK"))}", Encoding.GetEncoding("GBK"), "application/x-www-form-urlencoded")).Result;
                doc.Load(response.Content.ReadAsStreamAsync().Result);
            }
            var nodes = doc.DocumentNode.SelectNodes("//*[@id='main']/div[1]/ul/li");
            foreach (var node in nodes)
            {
                data.Add(new BookInfo(this)
                {
                    BookName = node.SelectSingleNode("span[2]/a").InnerText,
                    Category = node.SelectSingleNode("span[1]").InnerText.Trim('[', ']'),
                    Author = node.SelectSingleNode("span[4]").InnerText,
                    URL = new Uri(new Uri(url), node.SelectSingleNode("span[2]/a").GetAttributeValue("href", string.Empty)).ToString(),
                    Latestchapters = node.SelectSingleNode("span[3]/a").InnerText
                });
            }
            return data;
        }
    }
}
