using BookReading.Libs.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookReading.Libs
{
    public interface IFinder
    {
        /// <summary>
        /// 搜索器名称
        /// </summary>
        string FinderName { get; }

        /// <summary>
        /// 搜索器KEY
        /// </summary>
        Guid FinderKey { get; }

        /// <summary>
        /// 按关键字搜索小说
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<IList<BookModel>> SearchByKeywordAsync(string keyword);

        /// <summary>
        /// 获取章节
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<IList<ChapterModel>> GetChaptersAsync(string url);

        /// <summary>
        /// 获取章节段落
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<IList<string>> GetParagraphListAsync(string url);
    }
}
