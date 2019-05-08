using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Pages
{
    public class BookSearchViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IEnumerable<IFinder> finders;

        public BookSearchViewModel(IContainer container)
        {
            this.container = container;
            finders = container.GetAll<IFinder>();
        }

        public string Keyword { get; set; }

        public bool CanDoSearch => !string.IsNullOrEmpty(Keyword);

        public void DoSearch()
        {
            ConcurrentQueue<BookInfo> tempQueue = new ConcurrentQueue<BookInfo>();
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var tasks = finders.Select(finder => Task.Run(() =>
            {
                var list = finder.SearchByKeyword(Keyword);
                foreach (var item in list)
                {
                    tempQueue.Enqueue(item);
                }
            })).ToArray();
            Task.Run(() =>
            {
                while (true)
                {
                    if (tempQueue.TryDequeue(out BookInfo book))
                    {

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

        public void Cancel()
        {
            base.RequestClose(false);
        }

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
