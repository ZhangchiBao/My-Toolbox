using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BookApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpGet, Route("search/{keyword}/{pageIndex}")]
        public dynamic SearchByKeyword(string keyword, int pageIndex)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            var url = $"https://www.qidian.com/search?kw={keyword}&page={pageIndex}";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"result-list\"]/div/ul/li");
            List<dynamic> data = new List<dynamic>();
            foreach (var node in nodes)
            {
                var coverNode = node.SelectSingleNode("div[@class='book-img-box']/a/img");
                var bookNameNode = node.SelectSingleNode("div[@class='book-mid-info']/h4/a");
                var authorNode = node.SelectSingleNode("div[@class='book-mid-info']/p[1]/a[1]");
                var introNode = node.SelectSingleNode("div[@class='book-mid-info']/p[2]");
                var updateTitleNode = node.SelectSingleNode("div[@class='book-mid-info']/p[3]/a");
                var updateTimeNode = node.SelectSingleNode("div[@class='book-mid-info']/p[3]/span");
                data.Add(new
                {
                    CoverURL = new Uri(new Uri(url), coverNode.GetAttributeValue("src", string.Empty)).ToString(),
                    Name = bookNameNode.InnerText,
                    Author = authorNode.InnerText,
                    Intro = introNode.InnerText.Trim('\r', '\n', ' '),
                    UpdateTitle = updateTitleNode.InnerText.Remove(0, 4).Trim(),
                    UpdateTime = updateTimeNode.InnerText
                });
            }
            return data;
        }
    }
}