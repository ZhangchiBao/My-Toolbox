using Book.Common;
using Book.Models;
using System.Collections.Generic;
using System.IO;

namespace Book.ExportMethods
{
    public class MobiExport : IExport
    {
        private const string KINDLEGEN_PATH = "kindlegen.exe";

        public string Extension => ".mobi";

        public string Title => "MOBI文件";

        public bool Export(string fileName, BookInfo book, IList<ChapterInfo> chapters)
        {
            var kindleGenInfo = new FileInfo(KINDLEGEN_PATH);
            if (!kindleGenInfo.Exists)
            {
                File.WriteAllBytes(KINDLEGEN_PATH, Properties.Resources.kindlegen);
            }
            EpubExport epubExport = new EpubExport();
            var fileInfo = new FileInfo(fileName);
            var epubName = Path.Combine(fileInfo.DirectoryName, fileInfo.Name.Replace(Extension, epubExport.Extension));
            if (epubExport.Export(epubName, book, chapters))
            {
                Cmder.Execute($"\"{kindleGenInfo.FullName}\" \"{epubName}\" -c2 -o {fileInfo.Name} -dont_append_source");
                File.Delete(epubName);
                if (File.Exists(fileName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
