using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookReading.Entities;
using Stylet;
using StyletIoC;

namespace BookReading.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        public ShellViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager) : base(container, windowManager, viewManager)
        {
            Task.Run(LoadBookShelf);
        }

        public ObservableCollection<CategoryShowModel> ShlefData { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 弹出搜索窗口
        /// </summary>
        public void ShowSearchView()
        {
            var vm = container.Get<SearchViewModel>();
            vm.Keyword = Keyword;
            if (!string.IsNullOrWhiteSpace(vm.Keyword))
            {
                vm.DoSearch();
            }
            if (windowManager.ShowDialog(vm) ?? false)
            {
                Task.Run(LoadBookShelf);
            }
        }

        public void Keyword_Inputbox_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    ShowSearchView();
                }
            }
        }

        /// <summary>
        /// 加载书架
        /// </summary>
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

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
            }
        }
    }
}
