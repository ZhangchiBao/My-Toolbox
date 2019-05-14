using Biblioteca_del_Papa.Finders;
using Stylet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Entities
{
    public class BookShowEntity : PropertyChangedBase
    {
        public BookShowEntity(Func<BookShowEntity, Task> renew)
        {
            RenewCommand = new Command(() =>
            {
                Renewing = true;
                renew(this).ContinueWith(task =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Renewing = false;
                    });
                });
            });
        }

        public int ID { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public IFinder Finder { get; set; }

        public string URL { get; set; }

        public ICommand RenewCommand { get; set; }

        public List<ChapterInfo> Chapters { get; set; }

        public bool Renewing { get; private set; }
    }
}
