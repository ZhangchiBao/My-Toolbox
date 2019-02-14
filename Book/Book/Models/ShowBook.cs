using Stylet;
using System.Collections.ObjectModel;

namespace Book.Models
{
    public class ShowBook : PropertyChangedBase
    {
        public BookInfo Book { get; set; }

        public ObservableCollection<ChapterInfo> ChapterList { get; set; }
    }
}
