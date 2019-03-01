namespace BookAPP.Entity
{
    public class SearchBookSourceResponse
    {
        public bool IsLastSite { get; set; }
        public BookSource Data { get; set; }
    }

    public class BookSource
    {
        public string URL { get; set; }
        public string SiteName { get; set; }
        public int SiteID { get; set; }
        public string Update { get; set; }
    }
}
