using BookAPP.Entity;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpGet, Route("search/{keyword}/{pageIndex}")]
        public List<SearchBookResponse> SearchByKeyword(string keyword, int pageIndex)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            var url = $"https://www.qidian.com/search?kw={keyword}&page={pageIndex}";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"result-list\"]/div/ul/li");
            List<SearchBookResponse> data = new List<SearchBookResponse>();
            foreach (var node in nodes)
            {
                var coverNode = node.SelectSingleNode("div[@class='book-img-box']/a/img");
                var bookNameNode = node.SelectSingleNode("div[@class='book-mid-info']/h4/a");
                var authorNode = node.SelectSingleNode("div[@class='book-mid-info']/p[1]/a[1]");
                var introNode = node.SelectSingleNode("div[@class='book-mid-info']/p[2]");
                var updateTitleNode = node.SelectSingleNode("div[@class='book-mid-info']/p[3]/a");
                var updateTimeNode = node.SelectSingleNode("div[@class='book-mid-info']/p[3]/span");
                data.Add(new SearchBookResponse
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

        [HttpGet, Route("getsource/{name}/{author}/{index}")]
        public SearchBookSourceResponse SearchBookSource(string name, string author, int index)
        {
            using (var db = new LibraryContext())
            {
                var site = db.Sites.OrderBy(a => a.ID).Skip(index - 1).FirstOrDefault();
                if (site == null)
                {
                    return new SearchBookSourceResponse
                    {
                        IsLastSite = true,
                        Data = null
                    };
                }
                for (int i = 0; i < 5; i++)
                {
                    HtmlWeb web = new HtmlWeb();
                    var url = site.SearchURL.Replace("[s]", name).Replace("[p]", i.ToString());
                    var doc = web.Load(url);
                    var resultNodes = doc.DocumentNode.SelectNodes(@"//" + site.BookResultsNode);
                    foreach (HtmlNode resultNode in resultNodes)
                    {
                        var bookName = resultNode.SelectSingleNode(site.BookNameNode).InnerText;
                        var authorName = resultNode.SelectSingleNode(site.AuthorNode)?.InnerText;
                        if (bookName.ToLower().Trim() == name.ToLower().Trim() && author.ToLower().Trim() == authorName.ToLower().Trim())
                        {
                            var href = resultNode.SelectSingleNode(site.BookURLNode).GetAttributeValue("href", string.Empty);
                            href = new Uri(new Uri(url), href).ToString();
                            if (new Uri(url).Host.Contains("qidian.com"))
                            {
                                href += "#Catalog";
                            }
                            string update = string.Empty;
                            if (!string.IsNullOrEmpty(site.UpdateNode))
                            {
                                update = resultNode.SelectSingleNode(site.UpdateNode)?.InnerText;
                            }
                            return new SearchBookSourceResponse
                            {
                                IsLastSite = index == db.Sites.Count(),
                                Data = new BookSource
                                {
                                    URL = href,
                                    SiteName = site.Name,
                                    SiteID = site.ID,
                                    Update = update
                                }
                            };
                        }
                    }
                }
                return new SearchBookSourceResponse
                {
                    IsLastSite = false,
                    Data = null
                };
            }
        }
    }
}