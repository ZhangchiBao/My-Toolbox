using Biblioteca_del_Papa.Finders;

namespace Biblioteca_del_Papa.Entities
{
    public class ChapterInfo
    {
        private IFinder finder;

        public ChapterInfo(IFinder finder)
        {
            this.finder = finder;
        }

        public string Title { get; set; }

        public string Content { get; set; }
        public string URL { get; internal set; }
    }
}
