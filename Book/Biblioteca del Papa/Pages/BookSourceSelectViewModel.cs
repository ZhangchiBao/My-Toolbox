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

        /// <summary>
        /// 搜索结果数据
        /// </summary>
        public SearchBookByKeywordResult Data { get; set; }

        /// <summary>
        /// 选中源
        /// </summary>
        public BookInfo SelectedSource { get; set; }

        /// <summary>
        /// 能否确定
        /// </summary>
        public bool CanConfirm => SelectedSource != null;

        /// <summary>
        /// 源列表双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            RequestClose(false);
        }

        /// <summary>
        /// 确定
        /// </summary>
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
                    db.Books.Add(new Book
                    {
                        CategoryID = category.ID,
                        BookName = SelectedSource.BookName,
                        Author = SelectedSource.Author,
                        FinderKey = SelectedSource.Finder.FinderKey,
                        URL = SelectedSource.URL,
                        CoverURL = SelectedSource.Cover
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
