using System;
using Windows.UI.Xaml.Controls;

namespace Book.App.Model
{
    public class NavigationItemModel
    {
        public string Title { get; set; }

        public Symbol Icon { get; set; }

        public string Tag { get; set; }
        public Type ViewType { get; internal set; }
    }
}
