using Biblioteca_del_Papa.Entities;
using Stylet;
using StyletIoC;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Biblioteca_del_Papa.Pages
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel(IContainer container, IViewManager viewManager)
        {
            TabItems = container.GetAll<ITabItem>().OrderBy(a => a.TabIndex).ToDictionary(a => a.TabTitle, a => viewManager.CreateAndBindViewForModelIfNecessary(a));
        }

        public Dictionary<string, UIElement> TabItems { get; }
    }
}
