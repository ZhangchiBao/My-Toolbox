using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Entities
{
    public class SearchBookByKeywordResult : PropertyChangedBase
    {
        public SearchBookByKeywordResult()
        {
            Data.CollectionChanged += Data_CollectionChanged;
        }

        private void Data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChange(nameof(Source));
        }

        public string BookName { get; set; }

        public string Author { get; set; }

        public ObservableCollection<BookInfo> Data { get; set; } = new ObservableCollection<BookInfo>();

        public string Source => string.Join(",", Data.Select(a => a.Finder.FinderName));

        public string Description { get; internal set; }
        public string Cover { get; internal set; }
    }
}
