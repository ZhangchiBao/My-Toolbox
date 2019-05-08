using Biblioteca_del_Papa.Entities;
using System;
using System.Collections.Generic;

namespace Biblioteca_del_Papa.Finders
{
    public interface IFinder
    {
        string FinderName { get; }

        Guid FinderKey { get; }

        IList<BookInfo> SearchByKeyword(string keyword);
    }
}
