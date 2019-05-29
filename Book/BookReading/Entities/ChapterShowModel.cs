using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading.Entities
{
    public class ChapterShowModel : PropertyChangedBase
    {
        public Guid ID { get; internal set; }
        public string Title { get; internal set; }
        public bool Downloaded { get; internal set; }
        public string FilePath { get; internal set; }
        public Guid FinderKey { get; internal set; }
        public int Index { get; internal set; }
    }
}
