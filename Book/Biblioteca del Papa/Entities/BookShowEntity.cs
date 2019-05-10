using Biblioteca_del_Papa.Finders;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Entities
{
    public class BookShowEntity : PropertyChangedBase
    {
        public BookShowEntity(Action<BookShowEntity> renew)
        {
            RenewCommand = new Command(() => { renew(this); });
        }

        public int ID { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public IFinder Finder { get; set; }

        public string URL { get; set; }

        public ICommand RenewCommand { get; set; }

        public List<ChapterInfo> Chapters { get; set; }
    }
}
