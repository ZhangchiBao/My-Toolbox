using Stylet;
using System;

namespace BookReading.Entities
{
    public class ChapterShowModel : PropertyChangedBase
    {
        /// <summary>
        /// 章节ID
        /// </summary>
        public Guid ID { get; internal set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// 是否已下载
        /// </summary>
        public bool Downloaded { get; internal set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath { get; internal set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        public Guid FinderKey { get; internal set; }

        /// <summary>
        /// 章节序号
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// 章节下载地址
        /// </summary>
        public string URL { get; set; }
    }
}
