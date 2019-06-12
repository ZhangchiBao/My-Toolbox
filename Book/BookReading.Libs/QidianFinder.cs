using BookReading.Libs.Entity;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookReading.Libs
{
    //public class QidianFinder : IFinder
    //{
    //    public string FinderName => "起点阅读";

    //    public Guid FinderKey => new Guid("5AF2D68B-C36F-48EA-8009-0EF650CBAF0E");

    //    //public IList<ChapterModel> GetChapters(string url)
    //    //{
    //    //    var data = new List<ChapterModel>();
    //    //    HtmlWeb web = new HtmlWeb();
    //    //    web.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Mobile Safari/537.36";
    //    //    web.UseCookies = true;
    //    //    var doc = web.Load(url);
    //    //    var nodes = doc.DocumentNode.SelectNodes("/li[@class='chapter-li']/a");
    //    //    if (nodes != null)
    //    //    {
    //    //        foreach (var node in nodes)
    //    //        {
    //    //            data.Add(new ChapterModel(finder: this)
    //    //            {
    //    //                Title = node.InnerText,
    //    //                URL = new Uri(new Uri(url), node.GetAttributeValue("href", string.Empty)).ToString()
    //    //            });
    //    //        }
    //    //    }

    //    //    return data;
    //    //}

    //    //public List<string> GetParagraphList(string url)
    //    //{
    //    //    HtmlWeb web = new HtmlWeb();
    //    //    var doc = web.Load(url);
    //    //    var nodes = doc.DocumentNode.SelectNodes($"//div[@class='text-wrap']/div/div[2]/p");
    //    //    return nodes.Select(node => node.InnerText.Trim()).ToList();
    //    //}

    //    //public IList<BookModel> SearchByKeyword(string keyword)
    //    //{
    //    //    var data = new List<BookModel>();
    //    //    HtmlWeb web = new HtmlWeb();
    //    //    web.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Mobile Safari/537.36";
    //    //    HtmlNodeCollection nodes;
    //    //    var url = $"https://m.qidian.com/search?kw={keyword}";
    //    //    var doc = web.Load(url);
    //    //    nodes = doc.DocumentNode.SelectNodes("//*[@id='books-']/li");
    //    //    foreach (var node in nodes)
    //    //    {
    //    //        var item = new BookModel(finder: this)
    //    //        {
    //    //            Author = node.SelectSingleNode("a/div/div[2]/div[1]/span").ChildNodes.Last(a => a.Name == "#text").InnerText.Replace("\n", "").Trim(),
    //    //            BookName = node.SelectSingleNode("a/div/div[1]/h4").InnerText,
    //    //            Category = node.SelectSingleNode("a/div/div[2]/div[2]/span/em[1]").InnerText,
    //    //            URL = new Uri(new Uri(url), node.SelectSingleNode("a").GetAttributeValue("href", string.Empty) + "#Catalog").ToString(),
    //    //            Cover = new Uri(new Uri(url), node.SelectSingleNode("a/img").GetAttributeValue("src", string.Empty)).ToString(),
    //    //            Description = node.SelectSingleNode("a/div/p").InnerText.Replace("\r", ""),
    //    //            //Latestchapters = node.SelectSingleNode("div[2]/p[3]/a").InnerText.Replace("最新更新  ", "")
    //    //        };
    //    //        item.Description = new Regex("[ ]+").Replace(item.Description, string.Empty);
    //    //        data.Add(item);
    //    //    }
    //    //    return data;
    //    //}
    //}
}
