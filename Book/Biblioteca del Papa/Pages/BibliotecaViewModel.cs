using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Stylet;
using StyletIoC;

namespace Biblioteca_del_Papa.Pages
{
    public class BibliotecaViewModel : Screen, ITabItem
    {
        private readonly IContainer container;
        private readonly IWindowManager windowManager;

        public BibliotecaViewModel(IContainer container, IWindowManager windowManager)
        {
            this.container = container;
            this.windowManager = windowManager;
        }

        public string TabTitle => "书库";

        public int TabIndex => 0;

        public void AddBook()
        {
            var searchViewModel = container.Get<BookSearchViewModel>();
            if (windowManager.ShowDialog(searchViewModel) ?? false)
            {

            }
        }
    }
}
