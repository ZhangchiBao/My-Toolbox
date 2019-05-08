using Biblioteca_del_Papa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Finders
{
    public class QidianFinder : IFinder
    {
        public string FinderName => "起点阅读";

        public Guid FinderKey => new Guid("5AF2D68B-C36F-48EA-8009-0EF650CBAF0E");

        public IList<BookInfo> SearchByKeyword(string keyword)
        {
            return new List<BookInfo>();
        }
    }
}
