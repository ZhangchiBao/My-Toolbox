using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Pages
{
    public class BookSourceSelectViewModel : Screen
    {
        private readonly IContainer container;

        public BookSourceSelectViewModel(IContainer container)
        {
            this.container = container;
        }

        public SearchBookByKeywordResult Data { get; set; }

        public BookInfo SelectedSource { get; set; }

        public bool CanConfirm => SelectedSource != null;

        public void SourceListDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is System.Windows.FrameworkElement element && element.DataContext is BookInfo book)
            {
                SelectedSource = book;
                if (CanConfirm)
                {
                    Confirm();
                }
            }
        }

        public void Cancel()
        {
            RequestClose(false);
        }

        public void Confirm()
        {
            if (MessageBox.Show($"要把小说《{SelectedSource.BookName}》({SelectedSource.Author})加入到书架么？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                using (var db = container.Get<DBContext>())
                {
                    var category = db.Categories.Include(a => a.Alias).SingleOrDefault(a => a.Alias.Any(b => b.AliasName == SelectedSource.Category));
                    if (category == null)
                    {
                        category = db.Categories.Single(a => a.CategoryName == "其他");
                    }
                    var finder = db.Finders.Single(a => a.Key == SelectedSource.Finder.FinderKey);
                    db.Books.Add(new Book
                    {
                        CategoryID = category.ID,
                        BookName = SelectedSource.BookName,
                        Author = SelectedSource.Author,
                        CurrentFinderID = finder.ID,
                        CurrentURL = SelectedSource.URL
                    });
                    db.SaveChanges();
                }
            }
            RequestClose(true);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedSource):
                    NotifyOfPropertyChange(nameof(CanConfirm));
                    break;
            }
        }
    }
}
