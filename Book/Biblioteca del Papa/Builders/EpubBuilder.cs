using Biblioteca_del_Papa.DAL;
using Newtonsoft.Json;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Biblioteca_del_Papa.Builders
{
    public class EpubBuilder : IBuilder
    {
        private readonly IContainer container;

        public EpubBuilder(IContainer container)
        {
            this.container = container;
        }

        public string BuilderName => "Epub";

        public string BuidlerSuffix => ".epub";

        public void Builde(int bookID)
        {
            using (var db = container.Get<DBContext>())
            {
                var book = db.Books.Include(b => b.Chapters).Single(a => a.ID == bookID);

                var floder = Path.Combine(App.APPFloder, book.BookName);
                if (!Directory.Exists(floder))
                {
                    Directory.CreateDirectory(floder);
                }

                var tempFloder = Path.Combine(floder, "temp");
                if (!Directory.Exists(tempFloder))
                {
                    Directory.CreateDirectory(tempFloder);
                }

                var mimetypeFile = Path.Combine(tempFloder, "mimetype");
                File.WriteAllText(mimetypeFile, "application/epub+zip", Encoding.UTF8);

                var META_INFFloder = Path.Combine(tempFloder, "META-INF");
                if (!Directory.Exists(META_INFFloder))
                {
                    Directory.CreateDirectory(META_INFFloder);
                }

                File.WriteAllText(Path.Combine(META_INFFloder, "container.xml"), Properties.Resources.container, Encoding.UTF8);

                var OEBPSFolder = Path.Combine(tempFloder, "OEBPS");
                if (!Directory.Exists(OEBPSFolder))
                {
                    Directory.CreateDirectory(OEBPSFolder);
                }

                File.WriteAllText(Path.Combine(OEBPSFolder, "style.css"), Properties.Resources.style);

                #region paragraph
                List<KeyValuePair<string, string>> chapterMap = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < book.Chapters.Count; i++)
                {
                    var chapter = book.Chapters[i];
                    var chapterTemplate = Properties.Resources.chapterTemplate;
                    var chapterContent = chapterTemplate.Replace("{ChapterTitle}", chapter.Title);
                    StringBuilder @string = new StringBuilder();
                    List<string> paragraphList = new List<string>();
                    if (!string.IsNullOrEmpty(chapter.Content))
                    {
                        paragraphList = JsonConvert.DeserializeObject<List<string>>(chapter.Content);
                    }

                    foreach (var paragraph in paragraphList)
                    {
                        @string.AppendLine($"<p>　　{paragraph}</p>");
                    }

                    chapterContent = chapterContent.Replace("{ChapterContent}", @string.ToString());
                    var chapterFile = $"chapter{i}";
                    File.WriteAllText(Path.Combine(OEBPSFolder, chapterFile + ".xhtml"), chapterContent, Encoding.UTF8);
                    chapterMap.Add(new KeyValuePair<string, string>(chapter.Title, chapterFile));
                } 
                #endregion

                #region toc.ncx
                var tocFile = Path.Combine(OEBPSFolder, "toc.ncx");
                var tocContent = Properties.Resources.tocTemplate;
                tocContent = tocContent.Replace("{BookTitle}", book.BookName);
                tocContent = tocContent.Replace("{Author}", book.Author);
                StringBuilder navMpa = new StringBuilder();
                for (int i = 0; i < chapterMap.Count; i++)
                {
                    navMpa.AppendLine($"<navPoint id=\"navpoint-{i}\" playOrder=\"{i}\">");
                    navMpa.AppendLine("<navLabel>");
                    navMpa.AppendLine($"<text>{chapterMap[i].Key}</text>");
                    navMpa.AppendLine("</navLabel>");
                    navMpa.AppendLine($"<content src=\"{chapterMap[i].Value}\"/>");
                    navMpa.AppendLine($"</navPoint>");
                }

                tocContent.Replace("{navMap}", navMpa.ToString());
                File.WriteAllText(tocFile, tocContent, Encoding.UTF8); 
                #endregion

                if (!string.IsNullOrEmpty(book.CoverURL))
                {
                    using (var client = new HttpClient())
                    {
                        var buffer = client.GetByteArrayAsync(book.CoverURL).Result;
                        File.WriteAllBytes(Path.Combine(OEBPSFolder, "cover.jpg"), buffer);
                    }
                }

                #region content.opf
                var contentFile = Path.Combine(OEBPSFolder, "content.opf");
                var contentContent = Properties.Resources.contentTemplate;
                contentContent = contentContent.Replace("{BookTitle}", book.BookName);
                contentContent = contentContent.Replace("{Author}", book.Author);
                contentContent = contentContent.Replace("{Date}", DateTime.Now.ToString("yyyy-MM-dd"));
                contentContent = contentContent.Replace("{AppName}", "");
                StringBuilder manifest = new StringBuilder();
                manifest.AppendLine("<item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\"/>");
                manifest.AppendLine("<item id=\"css\" href=\"style.css\" media-type=\"text/css\"/>");
                manifest.AppendLine("<item id=\"cover-image\" href=\"cover.jpg\" media-type=\"image/jpeg\"/>");
                StringBuilder spine = new StringBuilder();
                for (int i = 0; i < chapterMap.Count; i++)
                {
                    manifest.AppendLine($"<item id=\"{chapterMap[i].Value}\" href=\"{chapterMap[i].Value}.xhtml\" media-type=\"application/xhtml+xml\"/>");
                    spine.AppendLine($"<itemref idref=\"{chapterMap[i].Value}\"/>");
                }

                contentContent.Replace("{manifest}", manifest.ToString());
                contentContent = contentContent.Replace("{spine}", spine.ToString());
                File.WriteAllText(contentFile, contentContent, Encoding.UTF8); 
                #endregion
            }
        }
    }
}
