using Stylet;
using System;

namespace Book.Models
{
    public class ChapterInfo : PropertyChangedBase
    {
        public int ID { get; set; }

        public int BookID { get; set; }

        public string Title { get; set; }

        public string CurrentSRC { get; set; }

        public int SiteID { get; set; }

        public string Content { get; set; }
    }
}