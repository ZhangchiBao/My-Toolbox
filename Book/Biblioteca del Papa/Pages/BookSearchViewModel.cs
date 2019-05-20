using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Biblioteca_del_Papa.Pages
{
    public class BookSearchViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IWindowManager windowManager;
        private readonly IEnumerable<IFinder> finders;

        public BookSearchViewModel(IContainer container, IWindowManager windowManager)
        {
            this.container = container;
            this.windowManager = windowManager;
            finders = container.GetAll<IFinder>();
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 搜索结果
        /// </summary>
        public ObservableCollection<SearchBookByKeywordResult> SearchResults { get; set; } = new ObservableCollection<SearchBookByKeywordResult>();

        /// <summary>
        /// 能否执行搜索
        /// </summary>
        public bool CanDoSearch => !string.IsNullOrEmpty(Keyword);

        /// <summary>
        /// 执行搜索
        /// </summary>
        public void DoSearch()
        {
            ConcurrentQueue<BookInfo> tempQueue = new ConcurrentQueue<BookInfo>();
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var tasks = finders.Select(finder => Task.Run(() =>
            {
                try
                {
                    IList<BookInfo> list = finder.SearchByKeyword(Keyword);
                    foreach (var item in list)
                    {
                        tempQueue.Enqueue(item);
                    }
                }
                catch
                {

                }
            })).ToArray();
            Task.Run(() =>
            {
                while (true)
                {
                    if (tempQueue.TryDequeue(out BookInfo book))
                    {
                        var item = SearchResults.SingleOrDefault(a => a.BookName == book.BookName);
                        if (item == null)
                        {
                            item = new SearchBookByKeywordResult()
                            {
                                BookName = book.BookName,
                                Author = book.Author,
                                Cover = book.Cover,
                                Description = book.Description
                            };
                            View.Dispatcher.Invoke(() =>
                            {
                                SearchResults.Add(item);
                            });
                        }
                        if (string.IsNullOrEmpty(item.Cover))
                        {
                            item.Cover = book.Cover;
                        }
                        if (string.IsNullOrEmpty(item.Description))
                        {
                            item.Description = book.Description;
                        }
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            item.Data.Add(book);
                        });
                    }
                    else
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            }, token);
            Task.WhenAll(tasks).ContinueWith(task =>
            {
                tokenSource.Cancel();
            });
        }

        /// <summary>
        /// 输入框按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InputKeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CanDoSearch)
                {
                    DoSearch();
                }
            }
        }

        /// <summary>
        /// 列表双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is SearchBookByKeywordResult result)
            {
                BookSourceSelectViewModel bookSourceSelectViewModel = container.Get<BookSourceSelectViewModel>();
                bookSourceSelectViewModel.Data = result;
                if (windowManager.ShowDialog(bookSourceSelectViewModel) ?? false)
                {
                    RequestClose(true);
                }
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            base.RequestClose(false);
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Determine()
        {
            RequestClose(true);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Keyword):
                    NotifyOfPropertyChange(nameof(CanDoSearch));
                    break;
            }
        }
    }
}
