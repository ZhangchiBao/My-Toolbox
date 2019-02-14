using AutoMapper;
using Book.Models;
using HtmlAgilityPack;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Book.Pages
{
    public class BookShowViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly SitesDBContext db;

        public BookShowViewModel(IViewManager viewManager, IContainer container, SitesDBContext db)
        {
            this.container = container;
            this.viewManager = viewManager;
            this.db = db;
        }

        public BookInfo CurrentBook { get; set; }

        public ChapterInfo CurrentChapter { get; set; }

        public ChapterInfo NextChapter { get; set; }

        public bool CanGotoNextChapter => NextChapter != null;

        public ObservableCollection<ChapterInfo> ChapterList { get; set; } = new ObservableCollection<ChapterInfo>();

        public int ShowType { get; set; } = 0;

        public Visibility ChapterVisibility { get; set; } = Visibility.Collapsed;

        public Visibility CatalogVisibility { get; set; } = Visibility.Collapsed;

        public void GotoChapter(ChapterInfo chapterInfo)
        {
            Task.Run(() =>
            {
                container.Get<ShellViewModel>().IsBusy = true;
                using (var bookDB = new BookDBContext(CurrentBook.Name))
                {
                    CurrentChapter = Mapper.Map<ChapterInfo>(bookDB.Chapters.Single(a => a.ID == chapterInfo.ID));
                    NextChapter = Mapper.Map<ChapterInfo>(bookDB.Chapters.Where(a => a.BookID == CurrentChapter.BookID && a.ID > CurrentChapter.ID).FirstOrDefault());
                }

                if (string.IsNullOrEmpty(chapterInfo.Content))
                {
                    HtmlWeb web = new HtmlWeb();
                    var site = db.Sites.Single(a => a.ID == chapterInfo.SiteID);
                    var doc = web.Load(chapterInfo.CurrentSRC);
                    var content = doc.DocumentNode.SelectSingleNode(@"//" + site.ContentNode).InnerText;
                    content = content.Trim('\r').Trim('\n').Trim(' ').Replace("　　", "\r\n　　").Trim('\r').Trim('\n');
                    CurrentChapter.Content = content;
                    chapterInfo.Content = content;
                    db.SaveChanges();
                }
                ShowType = 2;
            }).ContinueWith(task =>
            {
                container.Get<ShellViewModel>().IsBusy = false;
            });
        }

        public void GotoNextChapter()
        {
            GotoChapter(NextChapter);
        }

        public void GotoCatalog()
        {
            ShowType = 1;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(CurrentBook):
                    Task.Run(() =>
                    {
                        container.Get<ShellViewModel>().IsBusy = true;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (CurrentBook != null)
                            {
                                using (var bookDB = new BookDBContext(CurrentBook.Name))
                                {
                                    ChapterList = new ObservableCollection<ChapterInfo>(bookDB.Chapters.Where(a => a.BookID == CurrentBook.ID).ToList().Select(a => Mapper.Map<ChapterInfo>(a)));
                                }

                                ShowType = 1;
                            }
                            else
                            {
                                ChapterList.Clear();
                                ShowType = 0;
                            }
                        });
                    }).ContinueWith(task =>
                    {
                        container.Get<ShellViewModel>().IsBusy = false;
                    });
                    break;
                case nameof(ShowType):
                    ChapterVisibility = ShowType == 2 ? Visibility.Visible : Visibility.Collapsed;
                    CatalogVisibility = ShowType == 1 ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case nameof(NextChapter):
                    NotifyOfPropertyChange(nameof(CanGotoNextChapter));
                    break;
            }
        }
    }
}
