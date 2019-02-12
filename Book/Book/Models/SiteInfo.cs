using Stylet;
using System;
using System.Runtime.Serialization;

namespace Book.Models
{
    [DataContract]
    public class SiteInfo : PropertyChangedBase
    {
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 站点主页
        /// </summary>
        [DataMember]
        public string URL { get; set; }

        /// <summary>
        /// 小说名称节点
        /// </summary>
        [DataMember]
        public string BookNameNode { get; set; }

        /// <summary>
        /// 作者节点
        /// </summary>
        [DataMember]
        public string AuthorNode { get; set; }

        /// <summary>
        /// 小说地址节点
        /// </summary>
        [DataMember]
        public string BookURLNode { get; set; }

        /// <summary>
        /// 搜索链接
        /// </summary>
        [DataMember]
        public string SearchURL { get; set; }

        /// <summary>
        /// 搜索容量
        /// </summary>
        [DataMember]
        public uint? SearchSize { get; set; }

        /// <summary>
        /// 搜索结果节点
        /// </summary>
        [DataMember]
        public string BookResultsNode { get; set; }

        /// <summary>
        /// 简介节点
        /// </summary>
        [DataMember]
        public string DescriptionNode { get; set; }

        /// <summary>
        /// 最新章节节点
        /// </summary>
        [DataMember]
        public string UpdateNode { get; set; }
    }
}
