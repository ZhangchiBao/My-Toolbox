namespace Book.App.ViewModel
{
    public class SearchViewModel : VMBase
    {
        private string _keyword;

        public SearchViewModel()
        {
        }

        public string Keyword { get => _keyword; set => Set(ref _keyword, value); }
    }
}
