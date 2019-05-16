using Biblioteca_del_Papa.Finders;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Entities
{
    public class ChapterShowEntity
    {
        public ChapterShowEntity(IFinder finder, int index, int maxIndex, Func<int, Task> gotoChapter, Func<Task> gotoCatelog)
        {
            this.Finder = finder;
            ShowChapterCommand = new Command(() =>
            {
                gotoChapter(index);
            });
            ShowLastChapterCommand = new Command(() =>
            {
                gotoChapter(index - 1);
            }, () =>
            {
                return index - 1 >= 0;
            });
            ShowNextChapterCommand = new Command(() =>
            {
                gotoChapter(index + 1);
            }, () =>
            {
                return index + 1 <= maxIndex;
            });
            ShowCatalogCommand = new Command(() =>
            {
                gotoCatelog();
            });
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string URL { get; set; }
        public IFinder Finder { get; private set; }

        public ICommand ShowChapterCommand { get; private set; }

        public ICommand ShowLastChapterCommand { get; private set; }

        public ICommand ShowNextChapterCommand { get; private set; }

        public ICommand ShowCatalogCommand { get; private set; }
    }
}
