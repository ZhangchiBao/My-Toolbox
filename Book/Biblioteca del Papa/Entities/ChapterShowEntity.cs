using Biblioteca_del_Papa.Finders;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Entities
{
    public class ChapterShowEntity
    {
        public ChapterShowEntity(IFinder finder, Func<ChapterShowEntity, Task> showChapter)
        {
            this.Finder = finder;
            ShowChapterCommand = new Command(() =>
            {
                showChapter(this);
            });
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public string URL { get; set; }
        public IFinder Finder { get; private set; }

        public ICommand ShowChapterCommand { get; set; }
    }
}
