using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Pages
{
    public class BibliotecaViewModel : Screen, ITabItem
    {
        private readonly IContainer container;
        private readonly IWindowManager windowManager;

        public BibliotecaViewModel(IContainer container, IWindowManager windowManager)
        {
            this.container = container;
            this.windowManager = windowManager;
            LoadBookrack();
        }

        /// <summary>
        /// Tab页标题
        /// </summary>
        public string TabTitle => "书库";

        /// <summary>
        /// Tab顺序
        /// </summary>
        public int TabIndex => 0;

        /// <summary>
        /// 书架分类
        /// </summary>
        public ObservableCollection<CategoryShowEntity> CategoryShowEntityCollection { get; set; }

        /// <summary>
        /// 主内容ViewModel
        /// </summary>
        public object MainContentViewModel { get; set; }

        /// <summary>
        /// 新增书本
        /// </summary>
        public void AddBook()
        {
            var searchViewModel = container.Get<BookSearchViewModel>();
            if (windowManager.ShowDialog(searchViewModel) ?? false)
            {
                LoadBookrack();
            }
        }

        /// <summary>
        /// 下载所有空白章节
        /// </summary>
        /// <param name="book"></param>
        public void DownloadAllBlankchapters(BookShowEntity book)
        {
            List<Task> tasks = new List<Task>();
            book.Updating = true;
            foreach (var chapter in book.Chapters)
            {
                if (string.IsNullOrEmpty(chapter.Content))
                {
                    tasks.Add(Task.Run(() =>
                    {
                        using (var db = container.Get<DBContext>())
                        {
                            var paragraphList = chapter.Finder.GetParagraphList(chapter.URL);
                            var dbChapter = db.Chapters.Single(a => a.ID == chapter.ID);
                            dbChapter.Content = JsonConvert.SerializeObject(paragraphList);
                            db.Entry(dbChapter).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }));
                }
            }
            Task.WhenAll(tasks).ContinueWith(task =>
            {
                LoadBookrack();
                var category = CategoryShowEntityCollection.Single(a => a.CategoryID == book.CategoryID);
                category.IsExpanded = true;
                var currentBook = category.Books.Single(a => a.ID == book.ID);
                currentBook.IsSelected = true;
            });
        }

        /// <summary>
        /// 更新目录
        /// </summary>
        /// <param name="book"></param>
        public void Renew(BookShowEntity book)
        {
            book.Updating = true;
            Task.Run(() =>
            {
                using (var db = container.Get<DBContext>())
                {
                    IList<ChapterInfo> chapters = book.Finder.GetChapters(book.URL);
                    var existChapterNames = book.Chapters.Select(a => a.Title).ToList();
                    var notExistChapters = chapters.Where(c => !existChapterNames.Contains(c.Title)).ToList();
                    db.Chapters.AddRange(notExistChapters.Select(a => new Chapter
                    {
                        Title = a.Title,
                        BookID = book.ID,
                        URL = a.URL,
                        FinderKey = book.Finder.FinderKey,
                        Content = a.Content
                    }));
                    db.SaveChanges();
                }
            }).ContinueWith(task =>
            {
                LoadBookrack();
                var category = CategoryShowEntityCollection.Single(a => a.CategoryID == book.CategoryID);
                category.IsExpanded = true;
                var currentBook = category.Books.Single(a => a.ID == book.ID);
                currentBook.IsSelected = true;

            });
        }

        /// <summary>
        /// 加载书架
        /// </summary>
        public void LoadBookrack()
        {
            var finders = container.GetAll<IFinder>();
            using (var db = container.Get<DBContext>())
            {
                var data = db.Categories.Include(a => a.Books).Include(a => a.Books.Select(b => b.Chapters)).ToList()
                    .Select(a => new CategoryShowEntity()
                    {
                        CategoryID = a.ID,
                        CategoryName = a.CategoryName,
                        Books = a.Books.Select(b => new BookShowEntity
                        {
                            Author = b.Author,
                            BookName = b.BookName,
                            Finder = finders.Single(f => f.FinderKey == b.FinderKey),
                            URL = b.URL,
                            ID = b.ID,
                            CategoryID = a.ID,
                            Chapters = b.Chapters.Select((c, index) => new ChapterShowEntity(index)
                            {
                                ID = c.ID,
                                Title = c.Title,
                                Finder = finders.Single(f => f.FinderKey == c.FinderKey),
                                Content = string.IsNullOrEmpty(c.Content) ? string.Empty : "\t" + string.Join(Environment.NewLine + "\t", JsonConvert.DeserializeObject<List<string>>(c.Content)),
                                URL = c.URL
                            }).ToList()
                        }).ToList()
                    }).ToList();
                CategoryShowEntityCollection = new ObservableCollection<CategoryShowEntity>(data);
            }
        }

        /// <summary>
        /// 树形图双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is System.Windows.FrameworkElement element && element.DataContext is BookShowEntity book)
            {
                var viewModel = container.Get<BookViewModel>();
                viewModel.CurrentBook = book;
                MainContentViewModel = viewModel;
            }
            else
            {
                MainContentViewModel = null;
            }
        }
    }
}
