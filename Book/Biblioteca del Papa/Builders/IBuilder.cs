using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Builders
{
    public interface IBuilder
    {
        string BuilderName { get; }

        string BuidlerSuffix { get; }

        void Builde(int bookID);
    }
}
