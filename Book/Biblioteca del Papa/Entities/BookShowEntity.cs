using Biblioteca_del_Papa.Finders;
using Stylet;
using System.Collections.Generic;

namespace Biblioteca_del_Papa.Entities
{
    public class BookShowEntity : PropertyChangedBase
    {
        public int ID { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public IFinder Finder { get; set; }

        public string URL { get; set; }

        public bool Updating { get; set; }

        public int CategoryID { get; set; }

        public bool IsSelected { get; set; }

        public List<ChapterShowEntity> Chapters { get; set; }
        public string CoverUrl { get; set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(IsSelected):
                    if (IsSelected)
                    {

                    }
                    break;
            }
        }
    }
}
