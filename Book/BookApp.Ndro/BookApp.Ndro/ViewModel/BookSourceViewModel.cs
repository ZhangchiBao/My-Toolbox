using BookApp.Ndro.Common;
using BookApp.Ndro.Control;
using BookApp.Ndro.View;
using BookAPP.Entity;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;

namespace BookApp.Ndro.ViewModel
{
    [View(typeof(BookSourcePage))]
    public class BookSourceViewModel : BaseViewModel
    {
        private BookModel _book;
        private ObservableCollection<BookSource> _bookSourceList;
        private LoadMoreStatus _loadStatus;

        public BookModel Book { get => _book; internal set => Set(ref _book, value); }

        public ObservableCollection<BookSource> BookSourceList { get => _bookSourceList; set => Set(ref _bookSourceList, value); }

        public LoadMoreStatus LoadStatus { get => _loadStatus; set => Set(ref _loadStatus, value); }

        public Command FlushSourceCommand => new Command(() =>
        {
            BookSourceList.Clear();
            FlushSource();
        });

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Book):
                    FlushSource();
                    break;
            }
        }

        private async void FlushSource()
        {            
            LoadStatus = LoadMoreStatus.StatusLoading;
            var index = 1;
            using (var client = new HttpClient())
            {
                SearchBookSourceResponse result = null;
                do
                {
                    var url = $"http://144.34.221.50:64445/api/book/getsource/{Book.Name}/{Book.Author}/{index++}";
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<SearchBookSourceResponse>(content);
                    if (result.Data != null)
                    {
                        BookSourceList.Add(result.Data);
                    }
                } while (!result.IsLastSite);
            }
            LoadStatus = LoadMoreStatus.StatusDefault;
        }
    }
}
