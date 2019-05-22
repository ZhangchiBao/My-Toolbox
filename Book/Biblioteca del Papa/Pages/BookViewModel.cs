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
        private readonly IWindowManager windowManager;

        public BookViewModel(IContainer container, IWindowManager windowManager)
        {
            this.container = container;
            this.windowManager = windowManager;
        }

        /// <summary>
        /// 当前小说
        /// </summary>
        public BookShowEntity CurrentBook { get; set; }

        /// <summary>
        /// 显示目录
        /// </summary>
        public bool ShowCatelog { get; set; }

        public bool InBusy { get; set; }

        /// <summary>
        /// 显示章节
        /// </summary>
        public bool ShowChapter => !ShowCatelog;

        /// <summary>
        /// 当前章节
        /// </summary>
        public ChapterShowEntity CurrentChapter { get; set; }

        /// <summary>
        /// 能否点击下一章
        /// </summary>
        public bool CanGotoNextChapter => CurrentChapter == null ? false : CurrentBook.Chapters.IndexOf(CurrentChapter) < CurrentBook.Chapters.Count - 1;

        /// <summary>
        /// 能否点击上一章
        /// </summary>
        public bool CanGotoLastChapter => CurrentChapter == null ? false : CurrentBook.Chapters.IndexOf(CurrentChapter) > 0;

        /// <summary>
        /// 点击下一章
        /// </summary>
        public void GotoNextChapter()
        {
            GotoChapter(CurrentChapter.Index + 1);
        }

        /// <summary>
        /// 点击下一章
        /// </summary>
        public void GotoLastChapter()
        {
            GotoChapter(CurrentChapter.Index - 1);
        }

        /// <summary>
        /// 换源下载章节内容
        /// </summary>
        public void ChangeSourceToDownloadContent()
        {
            var finders = container.GetAll<IFinder>();
            SearchBookByKeywordResult result = new SearchBookByKeywordResult
            {
                Author = CurrentBook.Author,
                BookName = CurrentBook.BookName
            };
            var tasks = finders.Select(finder => Task.Run(() =>
            {
                var searchResult = finder.SearchByKeyword(result.BookName);
                if (searchResult.Any(a => a.Author == result.Author && a.BookName == result.BookName))
                {
                    result.Data.Add(searchResult.Single(a => a.Author == result.Author && a.BookName == result.BookName));
                }
            })).ToArray();
            Task.WhenAll(tasks).ContinueWith(task =>
            {
                result.Data.Remove(result.Data.SingleOrDefault(a => a.Finder.FinderKey == CurrentBook.Finder.FinderKey));
                App.Current.Dispatcher.Invoke(() =>
                {
                    BookSourceSelectViewModel bookSourceSelectViewModel = container.Get<BookSourceSelectViewModel>();
                    bookSourceSelectViewModel.Data = result;
                    if (windowManager.ShowDialog(bookSourceSelectViewModel) ?? false)
                    {
                        var selectSource = bookSourceSelectViewModel.SelectedSource;
                        var chapters = selectSource.Finder.GetChapters(selectSource.URL);
                        for (int i = 0; i < chapters.Count; i++)
                        {
                            var chapter = chapters[i];
                            if (chapter.Title == CurrentChapter.Title)
                            {
                                using (var db = container.Get<DBContext>())
                                {
                                    var dbChapter = db.Chapters.SingleOrDefault(a => a.ID == CurrentChapter.ID);
                                    dbChapter.FinderKey = selectSource.Finder.FinderKey;
                                    dbChapter.URL = chapter.URL;
                                    dbChapter.Content = string.Empty;
                                    db.Entry(dbChapter).State = EntityState.Modified;
                                    db.SaveChanges();
                                    CurrentChapter.Finder = selectSource.Finder;
                                    CurrentChapter.Content = string.Empty;
                                    CurrentChapter.URL = chapter.URL;
                                    GotoChapter(i);
                                }
                                break;
                            }
                        }
                        RequestClose(true);
                    }
                });
            });
        }

        /// <summary>
        /// 切换章节
        /// </summary>
        /// <param name="index"></param>
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

        /// <summary>
        /// 重新下载章节内容
        /// </summary>
        public void ReDownloadContent()
        {
            InBusy = true;
            Task.Run(() =>
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
                InBusy = false;
            });
        }

        /// <summary>
        /// 前往目录页
        /// </summary>
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
