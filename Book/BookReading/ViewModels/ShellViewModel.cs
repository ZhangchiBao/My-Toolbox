using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookReading.Entities;
using StyletIoC;

namespace BookReading.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        public ShellViewModel(IContainer container) : base(container)
        {
            Task.Run(LoadBookShelf);
        }

        public ObservableCollection<CategoryShowModel> ShlefData { get; set; }

        private void LoadBookShelf()
        {
            var db = container.Get<BookContext>();
            var data = db.Categories.Include(a => a.Books).ToList().Select(category => new CategoryShowModel
            {
                ID = category.ID,
                Name = category.CategoryName,
                Books = new ObservableCollection<BookShowModel>(category.Books.Select(book => new BookShowModel
                {
                    ID = book.ID,
                    Name = book.BookName,
                    Author = book.Author,
                    Descption = book.Descption,
                    Cover = book.CoverURL,
                    FinderKey = book.FinderKey,
                    Chapters = new ObservableCollection<ChapterShowModel>(book.Chapters.Select((chapter, index) => new ChapterShowModel
                    {
                        Index = index,
                        ID = chapter.ID,
                        Title = chapter.Title,
                        Downloaded = chapter.Downloaded,
                        FilePath = chapter.FilePath,
                        FinderKey = chapter.FinderKey
                    }))
                }))
            });
            ShlefData = new ObservableCollection<CategoryShowModel>(data);
        }
    }
}
