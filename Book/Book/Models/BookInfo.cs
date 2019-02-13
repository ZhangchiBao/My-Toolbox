using Stylet;

namespace Book.Models
{
    public class BookInfo : PropertyChangedBase
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string CurrentSource { get; set; }

        public int CurrentSiteID { get; set; }
    }
}
