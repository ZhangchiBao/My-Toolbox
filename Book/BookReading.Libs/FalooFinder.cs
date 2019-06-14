using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BookReading.Libs.Entity;
using HtmlAgilityPack;

namespace BookReading.Libs
{
    public class FalooFinder : IFinder
    {
        public string FinderName => "飞卢小说网";

        public Guid FinderKey => new Guid("D7BB3C0F-FB22-4D0E-9269-43871669558B");

        public async Task<IList<ChapterModel>> GetChaptersAsync(string url)
        {
            using (var client = new HttpClient())
            {
                HtmlWeb web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(url, Encoding.GetEncoding("GB2312"));
                var nodes = doc.DocumentNode.SelectNodes("/html/body/div[8]/div[1]/div/div[3]/div[2]/table/tbody/tr/td/a");
                var data = new List<ChapterModel>();
                foreach (var node in nodes)
                {
                    data.Add(new ChapterModel(this)
                    {
                        URL = new Uri(new Uri(url), node.GetAttributeValue("href", string.Empty)).ToString(),
                        Title = node.InnerText
                    });
                }
                return data;
            }
        }

        public async Task<IList<string>> GetParagraphListAsync(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url, Encoding.GetEncoding("GB2312"));
            var nodes = doc.DocumentNode.SelectNodes("//*[@id='content']/text()");
            var data = new List<string>();
            foreach (var node in nodes)
            {
                data.Add(node.InnerText.Replace("&nbsp;", "").Trim());
            }
            return data;
        }

        public async Task<IList<BookModel>> SearchByKeywordAsync(string keyword)
        {
            var data = new List<BookModel>();
            using (var client = new HttpClient())
            {
                var url = $"https://b.faloo.com/l/0/1.html?t=1&k={HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"))}";
                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(responseContent);
                var nodes = doc.DocumentNode.SelectNodes("/html/body/div[@class='l_main']/div[@class='l_main0']/div[@class='l_main1']/div[@class='l_bar']");
                if (nodes == null || nodes.Count == 0)
                {
                    return data;
                }

                foreach (var node in nodes)
                {
                    data.Add(new BookModel(this)
                    {
                        BookName = node.SelectSingleNode("div[@class='l_rc']/div[1]/h1/a").InnerText,
                        Author = node.SelectSingleNode("div[@class='l_rc']/div[1]/span[1]/a").InnerText,
                        Category = node.SelectSingleNode("div[@class='l_rc']/div[1]/span[2]/a").InnerText,
                        Description = node.SelectSingleNode("div[@class='l_rc']/div[3]/a").InnerText,
                        URL = new Uri(new Uri(url), node.SelectSingleNode("div[@class='l_rc']/div[1]/h1/a").GetAttributeValue("href", string.Empty)).ToString(),
                        Cover = new Uri(new Uri(url), node.SelectSingleNode("div[@class='l_pic']/a/img").GetAttributeValue("src", string.Empty)).ToString()
                    });
                }
                return data;
            }
        }
    }
}
