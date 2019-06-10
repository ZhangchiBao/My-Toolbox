using BookReading.Libs.Entity;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookReading.Libs
{
    public class QidianFinder : IFinder
    {
        public string FinderName => "起点阅读";

        public Guid FinderKey => new Guid("5AF2D68B-C36F-48EA-8009-0EF650CBAF0E");

        public IList<ChapterModel> GetChapters(string url)
        {
            var data = new List<ChapterModel>();
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//*[@class='catalog-content-wrap']/div/div/ul/li/a");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    data.Add(new ChapterModel(finder: this)
                    {
                        Title = node.InnerText,
                        URL = new Uri(new Uri(url), node.GetAttributeValue("href", string.Empty)).ToString()
                    });
                }
            }

            return data;
        }

        public List<string> GetParagraphList(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes($"//div[@class='text-wrap']/div/div[2]/p");
            return nodes.Select(node => node.InnerText.Trim()).ToList();
        }

        public IList<BookModel> SearchByKeyword(string keyword)
        {
            var data = new List<BookModel>();
            HtmlWeb web = new HtmlWeb();
            int pageIndex = 1;
            HtmlNodeCollection nodes;
            do
            {
                var url = $"https://www.qidian.com/search?kw={keyword}&page={pageIndex}";
                var doc = web.Load(url);
                nodes = doc.DocumentNode.SelectNodes("//*[@id='result-list']/div/ul/li");
                pageIndex++;
                foreach (var node in nodes)
                {
                    var item = new BookModel(finder: this)
                    {
                        Author = node.SelectSingleNode("div[2]/p[1]/a[1]").InnerText,
                        BookName = node.SelectSingleNode("div[2]/h4/a").InnerText,
                        Category = node.SelectSingleNode("div[2]/p[1]/a[2]").InnerText,
                        URL = new Uri(new Uri(url), node.SelectSingleNode("div[2]/h4/a").GetAttributeValue("href", string.Empty) + "#Catalog").ToString(),
                        Cover = new Uri(new Uri(url), node.SelectSingleNode("div[1]/a/img").GetAttributeValue("src", string.Empty)).ToString(),
                        Description = node.SelectSingleNode("div[2]/p[2]").InnerText.Replace("\r", ""),
                        Latestchapters = node.SelectSingleNode("div[2]/p[3]/a").InnerText.Replace("最新更新  ", "")
                    };
                    item.Description = new Regex("[ ]+").Replace(item.Description, string.Empty);
                    data.Add(item);
                }
            } while (nodes != null && nodes.Count > 0 && pageIndex <= 2);
            return data;
        }
    }
}
