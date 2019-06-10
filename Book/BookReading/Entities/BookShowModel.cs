using Stylet;
using System;
using System.Collections.ObjectModel;

namespace BookReading.Entities
{
    public class BookShowModel : PropertyChangedBase
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public byte[] CoverContent { get; set; }

        public string CoverUrl { get; set; }
        public ObservableCollection<ChapterShowModel> Chapters { get; set; }
        public Guid FinderKey { get; set; }
        public string URL { get; set; }
    }
}
