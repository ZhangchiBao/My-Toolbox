using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa
{
    public static class EnumerableExtentions
    {
        public static int IndexOf<TV, TK>(this IList<TV> source, TV item, Func<TV, TK> converter)
        {
            return source.IndexOf(item, converter, converter);
        }

        public static int IndexOf<TS, TV, TK>(this IList<TS> source, TV item, Func<TS, TK> converterS, Func<TV, TK> converterV)
        {
            return source.Select(a => converterS(a)).ToList().IndexOf(converterV(item));
        }
    }
}
