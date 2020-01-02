using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceSearcher.UILogic.Models
{
    public class ResourceEntity
    {
        public string Name { get; internal set; }
        public string Link { get; internal set; }
        public bool IsMagnetURI { get; internal set; }
    }
}
