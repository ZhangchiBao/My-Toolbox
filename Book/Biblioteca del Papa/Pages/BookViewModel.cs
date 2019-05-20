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

        /// <summary>
        /// 当前小说
        /// </summary>
        public BookShowEntity CurrentBook { get; set; }

        /// <summary>
        /// 显示目录
        /// </summary>
        public bool ShowCatelog { get; set; }

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
        /// 换源下单章节
        /// </summary>
        public void ChangeSourceToDownloadContent()
        {
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
