using Stylet;
using System;
using System.Collections.Generic;

namespace BookReading.Entities
{
    public class ChapterShowModel : PropertyChangedBase
    {
        /// <summary>
        /// 章节ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否已下载
        /// </summary>
        public bool Downloaded { get; set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        public Guid FinderKey { get; set; }

        /// <summary>
        /// 段落
        /// </summary>
        public List<string> Sections { get; set; }

        /// <summary>
        /// 章节序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 章节下载地址
        /// </summary>
        public string URL { get; set; }
    }
}
