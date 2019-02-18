using Book.Common.Zip.Util;
using Book.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Book.ExportMethods
{
    public class EpubExport : IExport
    {
        public string Extension => ".epub";

        public string Title => "EPUB";

        public bool Export(string fileName, BookInfo book, IList<ChapterInfo> chapters)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            var directory = fileInfo.Directory.FullName;
            var tempDirectory = Path.Combine(directory, "temp");
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            try
            {
                #region 创建mimetype文件
                File.WriteAllText(Path.Combine(tempDirectory, "mimetype"), "application/epub+zip");
                #endregion

                #region 创建META-INF文件夹并在该文件夹下创建container.xml文件 
                var infDirectory = Path.Combine(tempDirectory, "META-INF");
                if (!Directory.Exists(infDirectory))
                {
                    Directory.CreateDirectory(infDirectory);
                }

                File.WriteAllText(Path.Combine(infDirectory, "container.xml"), Properties.Resources.container);
                #endregion

                var oebpsDirectory = Path.Combine(tempDirectory, "OEBPS");
                if (!Directory.Exists(oebpsDirectory))
                {
                    Directory.CreateDirectory(oebpsDirectory);
                }

                var manifestContent = new StringBuilder();
                var spineContent = new StringBuilder();
                var navMapContent = new StringBuilder();
                foreach (var chapter in chapters)
                {
                    manifestContent.AppendLine($"<item id=\"chapter{chapter.ID}\" href=\"chapter{chapter.ID}.xhtml\" media-type=\"application/xhtml+xml\"/>");
                    spineContent.AppendLine($"<itemref idref=\"chapter{chapter.ID}\"/>");
                    navMapContent.AppendLine($"<navPoint id=\"navpoint-{chapter.ID}\" playOrder=\"{chapter.ID}\">");
                    navMapContent.AppendLine($"<navLabel>");
                    navMapContent.AppendLine($"<text>{chapter.Title}</text>");
                    navMapContent.AppendLine($"</navLabel>");
                    navMapContent.AppendLine($"<content src=\"chapter{chapter.ID}.xhtml\"/>");
                    navMapContent.AppendLine($"</navPoint>");
                    var xhtml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"/>  <title>[Title]</title>  <link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\" /></head><body><p><b>[Title]</b></p>[Content]</body></html>";
                    xhtml = xhtml.Replace("[Title]", chapter.Title);
                    xhtml = xhtml.Replace("[Content]", string.Join("", chapter.Content.Split('\r', '\n').Select(a => $"<p>{a}</p>")));
                    File.WriteAllText(Path.Combine(oebpsDirectory, $"chapter{chapter.ID}.xhtml"), xhtml);
                }

                #region 创建content.opf文件
                var contentContent = new StringBuilder();
                contentContent.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?><package unique-identifier=\"小说下载阅读器\" xmlns=\"http://www.idpf.org/2007/opf\" version=\"2.0\">");
                contentContent.AppendLine("<metadata>");
                contentContent.AppendLine("<dc-metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:dcterms=\"http://purl.org/dc/terms/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                contentContent.AppendLine("<meta name=\"cover\" content=\"cover-image\"/>");
                contentContent.AppendLine($"<dc:title>{book.Name}</dc:title>");
                contentContent.AppendLine($"<dc:creator>{book.Author}</dc:creator>");
                contentContent.AppendLine($"<dc:contributor>小说下载阅读器</dc:contributor>");
                contentContent.AppendLine($"<dc:date>{DateTime.Today.ToString("yyyy-MM-dd")}</dc:date>");
                contentContent.AppendLine($"<dc:language>zh-CN</dc:language>");
                contentContent.AppendLine($"</dc-metadata>");
                contentContent.AppendLine("<x-metadata/>");
                contentContent.AppendLine("</metadata>");
                contentContent.AppendLine("<manifest>");
                contentContent.AppendLine("<item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\"/>");
                contentContent.AppendLine("<item id=\"css\" href=\"style.css\" media-type=\"text/css\"/>");
                //contentContent.AppendLine("<item id=\"cover-image\" href=\"cover.jpg\" media-type=\"image/jpeg\"/>");
                //contentContent.AppendLine("<item id=\"preface\" href=\"preface.xhtml\" media-type=\"application/xhtml+xml\"/>");
                contentContent.AppendLine(manifestContent.ToString());
                contentContent.AppendLine("</manifest>");
                contentContent.AppendLine("<spine toc=\"ncx\">");
                //contentContent.AppendLine("<itemref idref=\"preface\"/>");
                contentContent.AppendLine(spineContent.ToString());
                contentContent.AppendLine("</spine>");
                contentContent.AppendLine("<guide>");
                contentContent.AppendLine("<reference type=\"Other\" title=\"\" href=\"\"/>");
                contentContent.AppendLine("</guide>");
                contentContent.AppendLine("</package>");

                File.WriteAllText(Path.Combine(oebpsDirectory, "content.opf"), contentContent.ToString());
                File.WriteAllText(Path.Combine(oebpsDirectory, "content.html"), contentContent.ToString());
                #endregion

                #region 创建toc.ncx文件
                var tocContent = new StringBuilder();
                tocContent.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?><ncx version=\"2005-1\" xmlns=\"http://www.daisy.org/z3986/2005/ncx/\">");
                tocContent.AppendLine("<head>");
                tocContent.AppendLine("<meta name=\"dtb:uid\" content=\"\"/>");
                tocContent.AppendLine("<meta name=\"dtb:depth\" content=\"-1\"/>");
                tocContent.AppendLine("<meta name=\"dtb:totalPageCount\" content=\"0\"/>");
                tocContent.AppendLine("<meta name=\"dtb:maxPageNumber\" content=\"0\"/>");
                tocContent.AppendLine("</head>");
                tocContent.AppendLine("<docTitle>");
                tocContent.AppendLine($"<text>{book.Name}</text>");
                tocContent.AppendLine($"</docTitle>");
                tocContent.AppendLine("<docAuthor>");
                tocContent.AppendLine($"<text>{book.Author}</text>");
                tocContent.AppendLine($"</docAuthor>");
                tocContent.AppendLine("<navMap>");
                tocContent.AppendLine(navMapContent.ToString());
                tocContent.AppendLine("</navMap>");
                tocContent.AppendLine("</ncx>");

                File.WriteAllText(Path.Combine(oebpsDirectory, "toc.ncx"), tocContent.ToString());
                #endregion

                File.WriteAllText(Path.Combine(oebpsDirectory, "style.css"), Properties.Resources.style);
                ZipHelper.ZipDirectory(tempDirectory, fileName, false);
                Directory.Delete(tempDirectory, true);
                return true;
            }
            catch (Exception)
            {
                Directory.Delete(tempDirectory, true);
                return false;
            }
        }
    }
}
