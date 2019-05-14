using Biblioteca_del_Papa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca_del_Papa.TemplateSelectors
{
    public class BookShowTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CatalogTemplate { get; set; }

        public DataTemplate ChapterTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BookShowEntity book)
            {
                return CatalogTemplate;
            }
            else if (item is ChapterShowEntity)
            {
                return ChapterTemplate;
            }else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
