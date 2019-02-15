using Book.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Book.ExportMethods
{
    public interface IExport
    {
        string Extension { get; }

        string Title { get; }

        bool Export(string fileName, BookInfo book, IList<ChapterInfo> chapters);
    }
}
