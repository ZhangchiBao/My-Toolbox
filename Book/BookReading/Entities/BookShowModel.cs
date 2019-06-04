using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading.Entities
{
    public class BookShowModel : PropertyChangedBase
    {
        public Guid ID { get; internal set; }
        public string Name { get; internal set; }
        public string Author { get; internal set; }
        public string Descption { get; internal set; }
        public string Cover { get; internal set; }
        public ObservableCollection<ChapterShowModel> Chapters { get; internal set; }
        public Guid FinderKey { get; internal set; }
        public string BookFloder { get; internal set; }
    }
}
