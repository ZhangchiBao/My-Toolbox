using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Entities
{
    public interface ITabItem
    {
        string TabTitle { get; }

        int TabIndex { get; }
    }
}
