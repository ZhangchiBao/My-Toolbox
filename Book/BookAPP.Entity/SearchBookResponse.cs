using System.Collections.Generic;

namespace BookAPP.Entity
{
    /// <summary>
    /// 按关键字搜索小说响应信息实体
    /// </summary>
    public class SearchBookResponse : BaseResponse<List<BookModel>>
    {
    }

    public class BookModel
    {
        /// <summary>
        /// 封面地址
        /// </summary>
        public string CoverURL { get; set; }

        /// <summary>
        /// 小说名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 最后更新标题
        /// </summary>
        public string UpdateTitle { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
