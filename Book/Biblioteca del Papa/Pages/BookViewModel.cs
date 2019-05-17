using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Pages
{
    public class BookViewModel : Screen
    {
        private readonly IContainer container;

        public BookViewModel(IContainer container)
        {
            this.container = container;
        }

        public BookShowEntity CurrentBook { get; set; }

        public bool ShowCatelog { get; set; }

        public bool ShowChapter => !ShowCatelog;

        public ChapterShowEntity CurrentChapter { get; set; }

        public bool CanGotoNextChapter => CurrentChapter == null ? false : CurrentBook.Chapters.IndexOf(CurrentChapter) < CurrentBook.Chapters.Count - 1;

        public bool CanGotoLastChapter => CurrentChapter == null ? false : CurrentBook.Chapters.IndexOf(CurrentChapter) > 0;

        public void GotoNextChapter()
        {
            GotoChapter(CurrentChapter.Index + 1);
        }

        public void GotoLastChapter()
        {
            GotoChapter(CurrentChapter.Index - 1);
        }

        public void ChangeSourceToDownloadContent()
        {
        }

        public void GotoChapter(int index)
        {
            Task.Run(() =>
            {
                var chapter = CurrentBook.Chapters[index];
                if (string.IsNullOrWhiteSpace(chapter.Content))
                {
                    ReDownloadContent();
                }
                CurrentChapter = CurrentBook.Chapters[index];
                ShowCatelog = false;
            });
        }

        public async void ReDownloadContent()
        {
            await Task.Run(() =>
            {
                using (var db = container.Get<DBContext>())
                {
                    var paragraphList = CurrentChapter.Finder.GetParagraphList(CurrentChapter.URL);
                    CurrentChapter.Content = "\t" + string.Join(Environment.NewLine + "\t", paragraphList);
                    var dbChapter = db.Chapters.Single(a => a.ID == CurrentChapter.ID);
                    dbChapter.Content = JsonConvert.SerializeObject(paragraphList);
                    db.Entry(dbChapter).State = EntityState.Modified;
                    db.SaveChanges();
                }
            });
        }

        public async void GotoCatelog()
        {
            await Task.Run(() =>
            {
                ShowCatelog = true;
            });
        }

        protected override void NotifyOfPropertyChange([CallerMemberName] string propertyName = "")
        {
            base.NotifyOfPropertyChange(propertyName);
            switch (propertyName)
            {
                case nameof(CurrentBook):
                    ShowCatelog = true;
                    break;
                case nameof(ShowCatelog):
                    NotifyOfPropertyChange(nameof(ShowChapter));
                    break;
                case nameof(CurrentChapter):
                    NotifyOfPropertyChange(nameof(CanGotoLastChapter));
                    NotifyOfPropertyChange(nameof(CanGotoNextChapter));
                    break;
            }
        }
    }
}
