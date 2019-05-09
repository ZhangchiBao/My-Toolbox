using Biblioteca_del_Papa.Finders;

namespace Biblioteca_del_Papa.Entities
{
    public class BookInfo
    {
        public IFinder Finder { get; set; }

        public BookInfo(IFinder finder)
        {
            Finder = finder;
        }

        public string BookName { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string URL { get; set; }
        public string Cover { get; set; }
        public string Description { get; set; }
        public string Latestchapters { get; internal set; }
    }
}
