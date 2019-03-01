using System;

namespace BookAPP.Entity
{
    public class SearchBookResponse
    {
        public string CoverURL { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Intro { get; set; }
        public string UpdateTitle { get; set; }
        public string UpdateTime { get; set; }
    }
}
