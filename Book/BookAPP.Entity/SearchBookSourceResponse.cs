namespace BookAPP.Entity
{
    /// <summary>
    /// 搜索书源响应实体类
    /// </summary>
    public class SearchBookSourceResponse : BaseResponse<BookSource>
    {
        /// <summary>
        /// 是否最后一个站点
        /// </summary>
        public bool IsLastSite { get; set; } = true;
    }

    /// <summary>
    /// 书源信息
    /// </summary>
    public class BookSource
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// 最后更新
        /// </summary>
        public string Update { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
    }
}
