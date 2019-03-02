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
    [View(typeof(SearchPage))]
    public class SearchViewModel : BaseViewModel
    {
        private string _keyword;
        private int pageIndex;
        private ObservableCollection<BookModel> _data;
        private LoadMoreStatus _loadStatus;

        public SearchViewModel()
        {

        }

        public string Keyword { get => _keyword; set => Set(ref _keyword, value); }

        public ObservableCollection<BookModel> Data { get => _data; set => Set(ref _data, value); }

        public LoadMoreStatus LoadStatus { get => _loadStatus; set => Set(ref _loadStatus, value); }

        public Command SearchCommand => new Command(() =>
        {
            Data.Clear();
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
            if (o is BookModel book)
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
                var data = JsonConvert.DeserializeObject<SearchBookResponse>(content);
                if (data.IsSuccess)
                {
                    if (data.Data == null || data.Data.Count == 0)
                    {
                        LoadStatus = LoadMoreStatus.StatusNoData;
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StatusHasData;
                    }
                    if (data.Data != null && data.Data.Count > 0)
                    {
                        if (Data == null || Data.Count == 0)
                        {
                            Data = new ObservableCollection<BookModel>(data.Data);
                        }
                        else
                        {
                            foreach (var item in data.Data)
                            {
                                Data.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    await View.DisplayAlert("错误", data.Message, "关闭");
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
