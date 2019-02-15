using AutoMapper;
using Book.Models;
using HtmlAgilityPack;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Book.Pages
{
    public class BookShowViewModel : Screen
    {
        #region 私有字段
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly SitesDBContext db;
        #endregion

        public BookShowViewModel(IViewManager viewManager, IContainer container, SitesDBContext db)
        {
            this.container = container;
            this.viewManager = viewManager;
            this.db = db;
        }

        #region 公共属性
        /// <summary>
        /// 当前小说
        /// </summary>
        public BookInfo CurrentBook { get; set; }

        /// <summary>
        /// 当前章节
        /// </summary>
        public ChapterInfo CurrentChapter { get; set; }

        /// <summary>
        /// 下一章
        /// </summary>
        public ChapterInfo NextChapter { get; set; }

        /// <summary>
        /// 能否跳转下一章
        /// </summary>
        public bool CanGotoNextChapter => NextChapter != null;

        /// <summary>
        /// 章节列表
        /// </summary>
        public ObservableCollection<ChapterInfo> ChapterList { get; set; } = new ObservableCollection<ChapterInfo>();

        /// <summary>
        /// 显示类型
        /// 0 - 都不显示
        /// 1 - 显示目录
        /// 2 - 显示章节
        /// </summary>
        public int ShowType { get; set; } = 0;

        /// <summary>
        /// 章节视图是否显示
        /// </summary>
        public Visibility ChapterVisibility { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// 目录视图是否显示
        /// </summary>
        public Visibility CatalogVisibility { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// 上一章
        /// </summary>
        public ChapterInfo PreviousChapter { get; private set; }

        /// <summary>
        /// 能否跳转到上一章
        /// </summary>
        public bool CanGotoPreviousChapter => PreviousChapter != null;
        #endregion

        #region 公共方法
        /// <summary>
        /// 跳转到指定章节
        /// </summary>
        /// <param name="chapterInfo"></param>
        public async void GotoChapter(ChapterInfo chapterInfo)
        {
            await container.Get<ShellViewModel>().StartBusy(() =>
            {
                using (var bookDB = new BookDBContext(CurrentBook.Name))
                {
                    var currentChapter = bookDB.Chapters.Single(a => a.ID == chapterInfo.ID);
                    CurrentChapter = Mapper.Map<ChapterInfo>(currentChapter);
                    NextChapter = Mapper.Map<ChapterInfo>(bookDB.Chapters.Where(a => a.BookID == CurrentChapter.BookID && a.ID > CurrentChapter.ID).FirstOrDefault());
                    PreviousChapter = Mapper.Map<ChapterInfo>(bookDB.Chapters.Where(a => a.BookID == CurrentChapter.BookID && a.ID < CurrentChapter.ID).LastOrDefault());

                    if (string.IsNullOrEmpty(CurrentChapter.Content))
                    {
                        HtmlWeb web = new HtmlWeb();
                        var site = db.Sites.Single(a => a.ID == chapterInfo.SiteID);
                        var doc = web.Load(chapterInfo.CurrentSRC);
                        var content = doc.DocumentNode.SelectSingleNode(@"//" + site.ContentNode).InnerText;
                        content = "　　" + string.Join("\r\n　　", content.Split('\r', '\n').Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().Replace("　　", "\r\n　　")));
                        CurrentChapter.Content = content;
                        currentChapter.Content = content;
                        bookDB.SaveChanges();
                    }
                }
                ShowType = 2;
            });
        }

        /// <summary>
        /// 跳转到下一章
        /// </summary>
        public void GotoNextChapter()
        {
            GotoChapter(NextChapter);
        }

        /// <summary>
        /// 跳转到上一章
        /// </summary>
        public void GotoPreviousChapter()
        {
            GotoChapter(PreviousChapter);
        }

        /// <summary>
        /// 跳转到目录页
        /// </summary>
        public void GotoCatalog()
        {
            ShowType = 1;
        }

        /// <summary>
        /// 刷新章节
        /// </summary>
        public async void RefreshChapterList()
        {
            await container.Get<ShellViewModel>().StartBusy(() =>
            {
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
            });
        }
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(CurrentBook):
                    RefreshChapterList();
                    break;
                case nameof(ShowType):
                    ChapterVisibility = ShowType == 2 ? Visibility.Visible : Visibility.Collapsed;
                    CatalogVisibility = ShowType == 1 ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case nameof(NextChapter):
                    NotifyOfPropertyChange(nameof(CanGotoNextChapter));
                    break;
                case nameof(PreviousChapter):
                    NotifyOfPropertyChange(nameof(CanGotoPreviousChapter));
                    break;
            }
        }
    }
}
