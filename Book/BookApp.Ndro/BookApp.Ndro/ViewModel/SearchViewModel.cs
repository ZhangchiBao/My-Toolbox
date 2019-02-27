using System;
using BookApp.Ndro.Common;
using BookApp.Ndro.View;
using Xamarin.Forms;

namespace BookApp.Ndro.ViewModel
{
    [View(typeof(SearchPage))]
    public class SearchViewModel : BaseViewModel
    {
        private string _keyword;

        public SearchViewModel()
        {

        }

        public string Keyword { get => _keyword; set => Set(ref _keyword, value); }

        public Command SearchCommand => new Command(DoSearch);

        private void DoSearch()
        {
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
