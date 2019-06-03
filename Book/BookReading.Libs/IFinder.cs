using BookReading.Libs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading.Libs
{
    public interface IFinder
    {
        string FinderName { get; }

        Guid FinderKey { get; }

        IList<BookModel> SearchByKeyword(string keyword);

        IList<ChapterModel> GetChapters(string url);

        List<string> GetParagraphList(string url);
    }
}
