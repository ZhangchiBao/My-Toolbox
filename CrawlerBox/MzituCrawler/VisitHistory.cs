using System.Collections;
using System.Collections.Generic;

namespace MzituCrawler
{
    public delegate void VisitHistoryChanged<T>(IEnumerable<string> items);

    public class VisitHistory : IEnumerable<string>
    {
        private List<string> Data = new List<string>();

        public event VisitHistoryChanged<string> OnCollectionChanged;

        public void Add(string item)
        {
            Data.Add(item);
            OnCollectionChanged?.Invoke(new List<string> { item });
        }

        public void AddRange(IEnumerable<string> items)
        {
            Data.AddRange(items);
            OnCollectionChanged?.Invoke(items);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
