using BookApp.Ndro.Common;
using BookApp.Ndro.Model;
using BookApp.Ndro.View;
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
        private ObservableCollection<BookSearchResultModel> _data;

        public SearchViewModel()
        {

        }

        public string Keyword { get => _keyword; set => Set(ref _keyword, value); }

        public ObservableCollection<BookSearchResultModel> Data { get => _data; set => Set(ref _data, value); }

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

        private async void DoSearch()
        {
            var mainView = ViewManager.CreateView<MainPage>();
            mainView.IsBusy = true;
            var url = $"http://144.34.221.50:64445/api/book/search/{Keyword}/{pageIndex}";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<BookSearchResultModel>>(content);
                if (Data == null)
                {
                    Data = new ObservableCollection<BookSearchResultModel>(data);
                }
                else
                {
                    foreach (var item in data)
                    {
                        Data.Add(item);
                    }
                }
            }
            mainView.IsBusy = false;
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
