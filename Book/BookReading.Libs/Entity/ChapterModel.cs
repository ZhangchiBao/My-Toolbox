namespace BookReading.Libs.Entity
{
    public class ChapterModel
    {
        public ChapterModel(IFinder finder)
        {
            this.Finder = finder;
        }

        public string Title { get; set; }
        public string URL { get; set; }
        public IFinder Finder { get; set; }
    }
}