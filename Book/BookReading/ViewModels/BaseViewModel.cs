using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading.ViewModels
{
    public class BaseViewModel : Screen
    {
        protected readonly IContainer container;
        protected readonly IWindowManager windowManager;
        protected readonly IViewManager viewManager;

        public BaseViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager)
        {
            this.container = container;
            this.windowManager = windowManager;
            this.viewManager = viewManager;
        }
    }
}
