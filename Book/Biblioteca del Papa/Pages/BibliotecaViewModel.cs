﻿using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        public string TabTitle => "书库";

        public int TabIndex => 0;

        public BookShowEntity CurrentBook { get; set; }

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
        /// 加载书架
        /// </summary>
        public void LoadBookrack()
        {
            var finders = container.GetAll<IFinder>();
            using (var db = container.Get<DBContext>())
            {
                var data = db.Categories.Include(a => a.Books).Include(a => a.Books.Select(b => b.Chapters)).Include(a => a.Books.Select(b => b.Finder)).ToList()
                    .Select(a => new CategoryShowEntity()
                    {
                        CategoryID = a.ID,
                        CategoryName = a.CategoryName,
                        Books = a.Books.Select(b => new BookShowEntity(RenewAsync)
                        {
                            Author = b.Author,
                            BookName = b.BookName,
                            Finder = finders.Single(f => f.FinderKey == b.Finder.Key),
                            URL = b.URL,
                            ID = b.ID,
                            Chapters = b.Chapters.Select(c => new ChapterShowEntity(finders.Single(f => f.FinderKey == b.Finder.Key), ShowChapterAsync)
                            {
                                Title = c.Title,
                                Content = c.Content,
                                URL = c.URL
                            }).ToList()
                        }).ToList()
                    }).ToList();
                CategoryShowEntityCollection = new ObservableCollection<CategoryShowEntity>(data);
            }
        }

        private async Task ShowChapterAsync(ChapterShowEntity chapter)
        {
            await Task.Run(() =>
            {
                MainContentViewModel = chapter;
            });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private async Task RenewAsync(BookShowEntity book)
        {
            await Task.Run(() =>
             {
                 using (var db = container.Get<DBContext>())
                 {
                     IList<ChapterInfo> chapters = book.Finder.GetChapters(book.URL);
                     var finder = db.Finders.Single(a => a.Key == book.Finder.FinderKey);
                     var existChapterNames = book.Chapters.Select(a => a.Title).ToList();
                     var notExistChapters = chapters.Where(c => !existChapterNames.Contains(c.Title)).ToList();
                     db.Chapters.AddRange(notExistChapters.Select(a => new Chapter
                     {
                         Title = a.Title,
                         BookID = book.ID,
                         URL = a.URL,
                         FinderID = finder.ID,
                         Content = a.Content
                     }));
                     db.SaveChanges();
                 }
             });
        }

        public void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is System.Windows.FrameworkElement element && element.DataContext is BookShowEntity book)
            {
                CurrentBook = book;
                //var viewModel = container.Get<CatalogViewModel>();
                //viewModel.Book = CurrentBook;
                MainContentViewModel = CurrentBook;
            }
            else
            {
                MainContentViewModel = null;
            }
        }
    }
}
