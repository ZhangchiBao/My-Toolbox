using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

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
            LoadBookrack();
        }

        public void LoadBookrack()
        {
            using (var db = container.Get<DBContext>())
            {
                var data = db.Categories.Include(a => a.Books)
                    .Select(a => new Bookrack()
                    {
                        CategoryID = a.ID,
                        CategoryName = a.CategoryName,
                        Books = a.Books
                    }).ToList();
                BookrackData = new ObservableCollection<Bookrack>(data);
            }
        }

        public string TabTitle => "书库";

        public int TabIndex => 0;

        public ObservableCollection<Bookrack> BookrackData { get; set; }

        public void AddBook()
        {
            var searchViewModel = container.Get<BookSearchViewModel>();
            if (windowManager.ShowDialog(searchViewModel) ?? false)
            {
                LoadBookrack();
            }
        }
    }
}
