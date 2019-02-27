using BookApp.Ndro.Common;

namespace BookApp.Ndro.ViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        private string _keyword;

        public string Keyword { get => _keyword; set => Set(ref _keyword, value); }
    }
}
