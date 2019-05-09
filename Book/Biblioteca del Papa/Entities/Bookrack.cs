using Biblioteca_del_Papa.DAL;
using System.Collections.Generic;

namespace Biblioteca_del_Papa.Entities
{
    public class Bookrack
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<Book> Books { get; set; }
    }
}
