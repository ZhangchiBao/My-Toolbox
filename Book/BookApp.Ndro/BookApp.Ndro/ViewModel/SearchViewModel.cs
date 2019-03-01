using BookApp.Ndro.Common;
using BookApp.Ndro.Control;
using BookApp.Ndro.View;
using BookAPP.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Forms;

namespace BookApp.Ndro.ViewModel
{
    [View(typeof(SearchPage))]
    public class SearchViewModel : BaseViewModel
    {
        private string _keyword;
        private int pageIndex;
        private ObservableCollection<SearchBookResponse> _data;
        private LoadMoreStatus _loadStatus;

        public SearchViewModel()
        {

        }

        public string Keyword { get => _keyword; set => Set(ref _keyword, value); }

        public ObservableCollection<SearchBookResponse> Data { get => _data; set => Set(ref _data, value); }

        public LoadMoreStatus LoadStatus { get => _loadStatus; set => Set(ref _loadStatus, value); }

        public Command SearchCommand => new Command(() =>
        {
            pageIndex = 1;
            DoSearch();
        });

        public Command LoadNextPageCommand => new Command(() =>
        {
            pageIndex++;
            DoSearch();
        });

        public Command ItemTappedCommand => new Command(async o =>
        {
            if (o is SearchBookResponse book)
            {
                var booksourceViewModel = IOC.Get<BookSourceViewModel>();
                booksourceViewModel.BookSourceList = new ObservableCollection<BookSource>();
                await View.Navigation.PushAsync(ViewManager.CreateView<BookSourcePage>());
                booksourceViewModel.Book = book;
            }
        });

        private async void DoSearch()
        {
            LoadStatus = LoadMoreStatus.StatusLoading;
            var url = $"http://144.34.221.50:64445/api/book/search/{Keyword}/{pageIndex}";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<SearchBookResponse>>(content);
                if (data == null || data.Count == 0)
                {
                    LoadStatus = LoadMoreStatus.StatusNoData;
                }
                else
                {
                    LoadStatus = LoadMoreStatus.StatusHasData;
                }

                if (Data == null)
                {
                    Data = new ObservableCollection<SearchBookResponse>(data);
                }
                else
                {
                    foreach (var item in data)
                    {
                        Data.Add(item);
                    }
                }
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Keyword):
                    NotifyPropertyChange(nameof(SearchCommand));
                    break;
            }
        }
    }
}
