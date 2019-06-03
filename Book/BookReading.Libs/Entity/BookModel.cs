using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading.Libs.Entity
{
    public class BookModel
    {
        public BookModel(IFinder finder)
        {
            this.Finder = finder;
        }

        public string Author { get; internal set; }
        public string BookName { get; internal set; }
        public string Category { get; internal set; }
        public string URL { get; internal set; }
        public string Cover { get; internal set; }
        public string Description { get; internal set; }
        public string Latestchapters { get; internal set; }

        public IFinder Finder { get; }
    }
}
