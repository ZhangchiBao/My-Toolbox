using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ResourceSearcher.UILogic.Searchers
{
    public class HttpCommon
    {
        protected string GetHtml(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36");
                client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                var buffer = client.GetByteArrayAsync(url).Result;
                var temp = Encoding.UTF8.GetString(buffer);
                var reg = new Regex(@"charset=""(\S+)""");
                Encoding encoding = null;
                if (reg.IsMatch(temp))
                {
                    var match = reg.Match(temp);
                    var charset = match.Groups[1].Value;
                    encoding = Encoding.GetEncoding(charset);
                }
                if (encoding != null)
                {
                    return encoding.GetString(buffer);
                }
            }

            return string.Empty;
        }

        //protected string GetHtml(string url)
        //{
        //    return GetHtmlAsync(url).Result;
        //}
    }
}
