using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading.Entities
{
    public class CategoryShowModel : PropertyChangedBase
    {
        public ObservableCollection<BookShowModel> Books { get; internal set; }
        public string Name { get; internal set; }
        public int ID { get; internal set; }
    }
}
