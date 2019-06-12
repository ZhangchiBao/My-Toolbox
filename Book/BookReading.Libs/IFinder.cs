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

        Task<IList<BookModel>> SearchByKeywordAsync(string keyword);

        Task<IList<ChapterModel>> GetChaptersAsync(string url);

        Task<IList<string>> GetParagraphListAsync(string url);
    }
}
