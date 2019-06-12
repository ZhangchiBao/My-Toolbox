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

        public Task<IList<ChapterModel>> GetChaptersAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetParagraphListAsync(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<BookModel>> SearchByKeywordAsync(string keyword)
        {
            using (var client = new HttpClient())
            {
                var url = "http://www.fkzww.com/Book/Search.aspx";
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gb2312"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("SearchClass", "1"),
                    //new KeyValuePair<string, string>("SeaButton.x", "0"),
                    //new KeyValuePair<string, string>("SeaButton.x", "0"),
                    new KeyValuePair<string, string>("SearchKey", HttpUtility.UrlEncode(keyword,Encoding.GetEncoding("GB2312")))
                });
                var response = await client.PostAsync(url, formContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(responseContent);
                var nodes = doc.DocumentNode.SelectNodes("//*[@id='CListTitle']");
                return null;
            }
        }
    }
}
