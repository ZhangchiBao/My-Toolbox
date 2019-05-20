using Biblioteca_del_Papa.DAL;
using Stylet;
using System.Collections.Generic;

namespace Biblioteca_del_Papa.Entities
{
    /// <summary>
    /// 分类信息
    /// </summary>
    public class CategoryShowEntity : PropertyChangedBase
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public List<BookShowEntity> Books { get; set; }
    }
}
