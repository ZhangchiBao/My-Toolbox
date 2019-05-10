using Biblioteca_del_Papa.Finders;

namespace Biblioteca_del_Papa.Entities
{
    public class BookInfo
    {
        public IFinder Finder { get; set; }

        public BookInfo(IFinder finder)
        {
            Finder = finder;
        }

        /// <summary>
        /// 小说名称
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// 小说分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string URL { get; set; }
        
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 最新章节
        /// </summary>
        public string Latestchapters { get; internal set; }
    }
}
