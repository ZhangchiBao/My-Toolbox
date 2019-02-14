using BZ.WindowsService.Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace MeizituReptile
{
    public class PicReptilePlugin : IPlugin
    {
        public PluginData PluginData => new PluginData
        {
            Author = "",
            Name = "妹子图爬虫",
            Version = "v1.0"
        };

        private readonly string baseURL = @"http://www.meizitu.com/a/more_{0}.html";

        public void Start()
        {
            List<string> links = new List<string>();
            int pageIndex = 1;
            HtmlWeb web = new HtmlWeb();
            do
            {
                var doc = web.Load(string.Format(baseURL, pageIndex++));
                var linkNodes = doc.DocumentNode.SelectNodes(@"//ul[@class='wp-list clearfix']/li[@class='wp-item']/div[@class='con']/div[@class='pic']/a");
                foreach (var linkNode in linkNodes)
                {
                    var imgDoc = web.Load(linkNode.GetAttributeValue("href", string.Empty));
                    var title = imgDoc.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
                    var imageNodes = imgDoc.DocumentNode.SelectNodes("//div[@id='picture']/p/img");
                    foreach (var imageNode in imageNodes)
                    {
                        DownloadImage(imageNode.GetAttributeValue("src", string.Empty), title);
                    }
                }
            } while (links != null && links.Count >= 39);
        }

        private async void DownloadImage(string url, string title)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(url);
                var buffer = await client.GetByteArrayAsync(url);
                File.WriteAllBytes(uri.OriginalString, buffer);
            }
        }

        public void Stop()
        {
        }

        public void Test()
        {
            Start();
        }
    }
}
