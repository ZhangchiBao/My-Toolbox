using System;
using System.Collections.Generic;
using System.Text;

namespace BookApp.Ndro.Model
{
   public class BookSearchResultModel
    {
        public string CoverURL { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Intro { get; set; }

        public string UpdateTitle { get; set; }

        public string UpdateTime { get; set; }
    }
}
