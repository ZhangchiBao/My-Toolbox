using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;
using StyletIoC;

namespace BookReading.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager) : base(container, windowManager, viewManager)
        {
        }

        public string Keyword { get; set; }

        public bool CanDoSearch => !string.IsNullOrWhiteSpace(Keyword);

        public bool CanDownload => false;

        public void DoSearch()
        {

        }

        public void Exist()
        {
            base.RequestClose();
        }

        public void Download()
        {

        }
    }
}
