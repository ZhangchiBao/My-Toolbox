using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Xml.Linq;

namespace MzituCrawler
{
    internal class Program
    {
        private const int MINVALUE = 0x64;
        private const int MAXVALUE = 0x3E8;
        private static VisitHistory historyUrl = new VisitHistory();
        private static string historyFilePath;
        private static string logFilePath;

        private static void Main(string[] args)
        {
            var baseFolder = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "localPic", "Mzitu");
            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }
            historyFilePath = Path.Combine(baseFolder, "history.xml");
            logFilePath = Path.Combine(baseFolder, "log.log");
            if (File.Exists(historyFilePath))
            {
                using (var stream = new FileStream(historyFilePath, FileMode.Open, FileAccess.Read))
                {
                    XElement element = XElement.Load(stream);
                    var children = element.Elements();
                    foreach (var item in children)
                    {
                        historyUrl.Add(item.Value);
                    }
                }
            }
            historyUrl.OnCollectionChanged += o => SaveHistory();
            var baseUrl = $"https://www.mzitu.com";
            HtmlWeb web = new HtmlWeb();
            var indexDoc = web.Load(baseUrl);
            var pageNode = indexDoc.DocumentNode.SelectNodes("/html/body/div[@class='main']/div[@class='main-content']/div[@class='postlist']/nav/div/a").Last(a => a.GetAttributeValue("class", string.Empty) == "page-numbers");
            var pageCount = int.Parse(pageNode.InnerText);
            for (int pageIndex = 1; pageIndex <= pageCount; pageIndex++)
            {
                var url = new Uri(new Uri(baseUrl), $"/page/{pageIndex}/").ToString();
                var doc = web.Load(url);
                var nodes = doc.DocumentNode.SelectNodes("//*[@id='pins']/li/a");
                if (nodes.Count > 0x0)
                {
                    foreach (var node in nodes)
                    {
                        var title = node.SelectSingleNode("img").GetAttributeValue("alt", string.Empty);
                        var href = node.GetAttributeValue("href", string.Empty);
                        href = new Uri(new Uri(baseUrl), href).ToString();
                        DownloadImages(downloadFolder: Path.Combine(baseFolder, title), url: href);
                        Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(MINVALUE, MAXVALUE));
                    }
                    Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(MINVALUE, MAXVALUE));
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 保存到历史记录
        /// </summary>
        private static void SaveHistory()
        {
            XElement element = new XElement("Urls", historyUrl.Select(a => new XElement("url", a)));
            File.WriteAllText(historyFilePath, element.ToString());
        }

        private static void DownloadImages(string downloadFolder, string url)
        {
            if (historyUrl.Contains(url))
            {
                return;
            }
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }
            HtmlWeb web = new HtmlWeb();
            var indexDoc = web.Load(url);
            var pageNode = indexDoc.DocumentNode.SelectNodes("/html/body/div[@class='main']/div[@class='content']/div[@class='pagenavi']/a").Reverse().Skip(1).First();
            var pageCount = pageNode == null ? 1 : int.Parse(pageNode.InnerText);
            for (int pageIndex = 1; pageIndex <= pageCount; pageIndex++)
            {
                var doc = web.Load($"{url}/{pageIndex}");
                var imageNode = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/div[3]/p/a/img");
                if (imageNode != null)
                {
                    var imageUrl = imageNode.GetAttributeValue("src", string.Empty);
                    imageUrl = new Uri(new Uri(url), imageUrl).ToString();
                    if (historyUrl.Contains(imageUrl))
                    {
                        continue;
                    }
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Host = "i.meizitu.net";
                        client.DefaultRequestHeaders.Pragma.ParseAdd("no-cache");
                        client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate");
                        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("zh-CN,zh;q=0.8,en;q=0.6");
                        client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { NoCache = true };
                        client.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
                        client.DefaultRequestHeaders.Referrer = new Uri(url);
                        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36");
                        client.DefaultRequestHeaders.Accept.ParseAdd("image/webp,image/apng,image/*,*/*;q=0.8");
                        var buffer = client.GetByteArrayAsync(imageUrl).Result;
                        var fileName = new Uri(imageUrl).Segments.Last();
                        File.WriteAllBytes(Path.Combine(downloadFolder, fileName), buffer);
                        historyUrl.Add(imageUrl);
                    }
                    Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(MINVALUE, MAXVALUE));
                }
            }
            historyUrl.Add(url);
        }
    }
}
