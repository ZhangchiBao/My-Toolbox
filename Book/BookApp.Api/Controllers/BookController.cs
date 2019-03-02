using BookAPP.Entity;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookApp.Api.Controllers
{
    /// <summary>
    /// 小说相关接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        /// <summary>
        /// 按关键字搜索小说
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        [HttpGet("search/{keyword}/{pageIndex}")]
        public SearchBookResponse SearchByKeyword(string keyword, int pageIndex)
        {
            SearchBookResponse response = new SearchBookResponse();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                response.Message = "搜索关键字不能为空";
                return response;
            }

            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            var url = $"https://www.qidian.com/search?kw={keyword}&page={pageIndex}";
            response.Data = new List<BookModel>();
            response.IsSuccess = true;
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"result-list\"]/div/ul/li");
            foreach (var node in nodes)
            {
                var coverNode = node.SelectSingleNode("div[@class='book-img-box']/a/img");
                var bookNameNode = node.SelectSingleNode("div[@class='book-mid-info']/h4/a");
                var authorNode = node.SelectSingleNode("div[@class='book-mid-info']/p[1]/a[1]");
                var introNode = node.SelectSingleNode("div[@class='book-mid-info']/p[2]");
                var updateTitleNode = node.SelectSingleNode("div[@class='book-mid-info']/p[3]/a");
                var updateTimeNode = node.SelectSingleNode("div[@class='book-mid-info']/p[3]/span");
                response.Data.Add(new BookModel
                {
                    CoverURL = new Uri(new Uri(url), coverNode.GetAttributeValue("src", string.Empty)).ToString(),
                    Name = bookNameNode.InnerText,
                    Author = authorNode.InnerText,
                    Intro = introNode.InnerText.Trim('\r', '\n', ' '),
                    UpdateTitle = updateTitleNode.InnerText.Remove(0, 4).Trim(),
                    UpdateTime = updateTimeNode.InnerText
                });
            }
            return response;
        }

        /// <summary>
        /// 搜索书源
        /// </summary>
        /// <param name="name">书名</param>
        /// <param name="author">作者</param>
        /// <param name="index">序号</param>
        /// <returns></returns>
        [HttpGet("getsource/{name}/{author}/{index}")]
        public SearchBookSourceResponse SearchBookSource(string name, string author, int index)
        {
            SearchBookSourceResponse response = new SearchBookSourceResponse();
            if (string.IsNullOrWhiteSpace(name))
            {
                response.Message = "小说名称不能为空";
                return response;
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                response.Message = "小说作者不能为空";
                return response;
            }

            if (index <= 0)
            {
                index = 1;
            }

            using (var db = new LibraryContext())
            {
                response.IsSuccess = true;
                response.IsLastSite = index >= db.Sites.Count();
                var site = db.Sites.OrderBy(a => a.ID).Skip(index - 1).FirstOrDefault();
                if (site != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        HtmlWeb web = new HtmlWeb();
                        var url = site.SearchURL.Replace("[s]", name).Replace("[p]", i.ToString());
                        var doc = web.Load(url);
                        var resultNodes = doc.DocumentNode.SelectNodes(site.BookResultsNode);
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
                                response.Data = new BookSource
                                {
                                    URL = href,
                                    BookName = name,
                                    Author = author,
                                    SiteName = site.Name,
                                    SiteID = site.ID,
                                    Update = update
                                };
                                return response;
                            }
                        }
                    }
                }
                return response;
            }
        }

        /// <summary>
        /// 按当前源获取章节信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        [HttpGet("getChapters/{siteId}")]
        public GetChaptersResponse GetChapters(string url, int siteId)
        {
            var response = new GetChaptersResponse();
            if (string.IsNullOrWhiteSpace(url))
            {
                response.Message = "地址不能为空";
            }
            using (var db = new LibraryContext())
            {
                var site = db.Sites.SingleOrDefault(a => a.ID == siteId);
                if (site == null)
                {
                    response.Message = "未找到对应站点信息";
                    return response;
                }
                response.Data = new List<ChapterModel>();
                response.IsSuccess = true;
                HtmlWeb web = new HtmlWeb();
                var doc = web.Load(url);
                var resultNodes = doc.DocumentNode.SelectNodes(site.ChapterNode);
                foreach (var node in resultNodes)
                {
                    var chapterNameNode = node.SelectSingleNode(site.ChapterNameNode);
                    var chapterURLNode = node.SelectSingleNode(site.ChapterUrlNode);
                    var name = chapterNameNode.InnerText;
                    var href = chapterURLNode.GetAttributeValue("href", string.Empty);
                    href = new Uri(new Uri(url), href).ToString();
                    response.Data.Add(new ChapterModel
                    {
                        Name = name,
                        URL = href,
                        SiteID = siteId
                    });
                }
            }
            return response;
        }

        /// <summary>
        /// 获取章节内容
        /// </summary>
        /// <param name="url">章节地址</param>
        /// <param name="siteId">小说站点</param>
        /// <returns></returns>
        [HttpGet("getChapterContent/{siteId}")]
        public GetChapterContentResponse GetChapterContent(string url, int siteId)
        {
            var response = new GetChapterContentResponse();
            if (string.IsNullOrWhiteSpace(url))
            {
                response.Message = "地址不能为空";
            }
            using (var db = new LibraryContext())
            {
                var site = db.Sites.SingleOrDefault(a => a.ID == siteId);
                if (site == null)
                {
                    response.Message = "未找到对应站点信息";
                    return response;
                }
                response.IsSuccess = true;
                HtmlWeb web = new HtmlWeb();
                var doc = web.Load(url);
                var chapterTitleNode = doc.DocumentNode.SelectSingleNode(site.ChapterTitleNode);
                var chapterParagraphNodes = doc.DocumentNode.SelectSingleNode(site.ChapterParagraphNode).ChildNodes;
                var paragraphList = new List<string>();
                foreach (var node in chapterParagraphNodes)
                {
                    if (!string.IsNullOrEmpty(node.InnerText) && !string.IsNullOrEmpty(node.InnerText.Trim('\n').Trim('\r').Trim()))
                    {
                        paragraphList.Add("\t" + node.InnerText.Trim('\n').Trim('\r').Trim());
                    }
                }
                response.Data = new ChapterContentModel
                {
                    Title = chapterTitleNode.InnerText,
                    Content = string.Join("\r\n", paragraphList)
                };
            }
            return response;
        }
    }
}