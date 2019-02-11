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

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string BookNameNode { get; set; }

        [DataMember]
        public string AuthorNode { get; set; }

        [DataMember]
        public string BookURLNode { get; set; }

        [DataMember]
        public string SearchURL { get; internal set; }
    }
}
