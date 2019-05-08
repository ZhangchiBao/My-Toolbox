using Biblioteca_del_Papa.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Finders
{
    public class UCTextFinder : IFinder
    {
        public string FinderName => "UC书盟";

        public Guid FinderKey => new Guid("233A5DE7-FB6B-4CC3-A27F-844BD9B1766E");

        public IList<BookInfo> SearchByKeyword(string keyword)
        {
            var data = new List<BookInfo>();
            var url = $"http://www.uctxt.com/modules/article/search.php?searchkey={System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GBK"))}";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//*[@id='main']/section/div[2]/ul/li");
            foreach (var node in nodes)
            {
                data.Add(new BookInfo
                {
                    BookName = node.SelectSingleNode("span[2]/a").InnerText
                });
            }
            return data;
        }
    }
}
