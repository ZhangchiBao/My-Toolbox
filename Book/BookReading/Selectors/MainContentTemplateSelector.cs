using BookReading.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookReading.Selectors
{
    public class MainContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CatalogTemplate { get; set; }
        public DataTemplate ChapterTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BookShowModel book)
            {
                return CatalogTemplate;
            }
            if (item is ChapterShowModel chapter)
            {
                return ChapterTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
