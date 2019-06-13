using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BookReading.Libs.Entity;
using HtmlAgilityPack;

namespace BookReading.Libs
{
    public class FKZWWFinder : IFinder
    {
        public string FinderName => "疯狂中文网";

        public Guid FinderKey => new Guid("19655589-6958-41E3-AE3A-8C8AB8061C91");

        public async Task<IList<ChapterModel>> GetChaptersAsync(string url)
        {
            using (var client = new HttpClient())
            {
                //var response = await client.GetAsync(url);
                //var responseContent = await response.Content.ReadAsStringAsync();
                HtmlWeb web = new HtmlWeb();
                //var doc = new HtmlDocument();
                //doc.LoadHtml(responseContent);
                var doc = await web.LoadFromWebAsync(url, Encoding.GetEncoding("GB2312"));
                var nodes = doc.DocumentNode.SelectNodes("//*[@id='BookText']/ul/li/a");
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

        public Task<IList<string>> GetParagraphListAsync(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<BookModel>> SearchByKeywordAsync(string keyword)
        {
            using (var client = new HttpClient())
            {
                var url = $"http://www.fkzww.com/Book/Search.aspx?SearchClass=1&SearchKey={HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"))}";
                var response = await client.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(responseContent);
                var nodes = doc.DocumentNode.SelectNodes("//*[@id='CListTitle']");
                var descriptionNodes = doc.DocumentNode.SelectNodes("//*[@id='CListText']");
                var data = new List<BookModel>();
                var index = 0;
                foreach (var node in nodes)
                {
                    data.Add(new BookModel(this)
                    {
                        BookName = node.SelectSingleNode("a[1]/b").InnerText,
                        Author = node.SelectSingleNode("a[2]").InnerText,
                        Category = node.SelectSingleNode("a[3]").InnerText,
                        Description = descriptionNodes[index].InnerText,
                        URL = new Uri(new Uri(url), node.SelectSingleNode("a[1]").GetAttributeValue("href", string.Empty).Replace("/Book", "/Html/Book/1").Replace("/Index.aspx", "/List.shtml")).ToString()
                    });
                }
                return data;
            }
        }
    }
}
